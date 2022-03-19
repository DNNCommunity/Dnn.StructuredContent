using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.CompressionTasks;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.Git.GitTasks;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;
using static Nuke.Common.Tools.Npm.NpmTasks;
using System.Xml;
using System.Globalization;
using Octokit;
using BuildHelpers;
using System.IO.Compression;
using System.IO;
using Nuke.Common.Tools.Npm;
using System.Text;
using System.Security.Cryptography;

[CheckBuildProjectConfigurations]
[ShutdownDotNetAfterServerBuild]
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.Package);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    [Parameter("Github Token")] readonly string GithubToken;

    [Solution] readonly Solution Solution;
    [GitRepository] readonly GitRepository GitRepository;
    [GitVersion] readonly GitVersion GitVersion;

    /// <summary>Defines the proper branch used for official releases in the git repository.</summary>
    private const string mainBranch = "main";

    /// <summary>The prefex for branches created for pre-releases (RCs).</summary>
    private const string releaseBranchPrefix = "release";

    /// <summary>The contents of the automated release notes.</summary>
    string releaseNotes = "";

    /// <summary>
    /// The owner of the repository.
    /// </summary>
    string repositoryOwner = "";

    /// <summary>
    /// The name of the repository
    /// </summary>
    string repositoryName = "";

    /// <summary>The name of the current branch in the repository.</summary>
    private string repositoryBranch = "";

    /// <summary>Defines the module name used in various places</summary>
    private const string moduleName = "Dnn.StructuredContent";

    /// <summary>A value indicating whether the current folder is withing a DNN website currently.</summary>
    private bool rootIsInDnn = RootDirectory.Parent.Parent.ToString().EndsWith("Dnn.PersonaBar", StringComparison.OrdinalIgnoreCase);

    /// <summary>The path to the frontend source files.</summary>
    private AbsolutePath webProjectDirectory = RootDirectory / "module.web";

    GitHubClient gitHubClient;
    Release release;

    /// <summary>
    /// Defines in which folder the package is packaged into.
    /// </summary>
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    /// <summary>
    /// Sets the proper git branch name.
    /// </summary>
    Target SetBranch => _ => _
        .Executes(() =>
        {
            repositoryBranch = GitRepository.Branch.StartsWith("refs/") ? GitRepository.Branch.Substring(11) : GitRepository.Branch;
            Serilog.Log.Information($"Set branch name to {repositoryBranch}");
        });

    /// <summary>
    /// Cleans the directories used for the build
    /// </summary>
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    /// <summary>
    /// Restores nuget packages
    /// </summary>
    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution.GetProject("Dnn.StructuredContent")));
        });

    /// <summary>
    /// Compiles the module dll.
    /// </summary>
    Target Compile => _ => _
        .DependsOn(Clean)
        .DependsOn(Restore)
        .DependsOn(SetBranch)
        .Executes(() =>
        {
            MSBuildTasks.MSBuild(s => s
                .SetProjectFile(Solution.GetProject("Dnn.StructuredContent"))
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(repositoryBranch == mainBranch ? GitVersion.MajorMinorPatch : GitVersion.AssemblySemVer)
                .SetFileVersion(repositoryBranch == mainBranch ? GitVersion.MajorMinorPatch : GitVersion.AssemblySemVer));
        });

    Target SetManifestVersions => _ => _
        .Executes(() =>
        {
            var manifests = GlobFiles(RootDirectory, "**/*.dnn");
            foreach (var manifest in manifests)
            {
                var doc = new XmlDocument();
                doc.Load(manifest);
                var packages = doc.SelectNodes("dotnetnuke/packages/package");
                foreach (XmlNode package in packages)
                {
                    var version = package.Attributes["version"];
                    if (version != null)
                    {
                        Serilog.Log.Information($"Found package {package.Attributes["name"].Value} with version {version.Value}");
                        version.Value = $"{GitVersion.Major.ToString("00", CultureInfo.InvariantCulture)}.{GitVersion.Minor.ToString("00", CultureInfo.InvariantCulture)}.{GitVersion.Patch.ToString("00", CultureInfo.InvariantCulture)}";
                        Serilog.Log.Information($"Updated package {package.Attributes["name"].Value} to version {version.Value}");
                    }
                }
                doc.Save(manifest);
                Serilog.Log.Information($"Saved {manifest}");
            }
        });

    /// <summary>
    /// Sets up the github client for github interactions.
    /// </summary>
    Target SetupGitHubClient => _ => _
        .OnlyWhenDynamic(() => !string.IsNullOrWhiteSpace(GithubToken))
        .DependsOn(SetBranch)
        .Executes(() =>
        {
            Serilog.Log.Information($"We are on branch {repositoryBranch}");
            if (repositoryBranch == "master" || repositoryBranch.StartsWith("release"))
            {
                repositoryOwner = GitRepository.Identifier.Split('/')[0];
                repositoryName = GitRepository.Identifier.Split('/')[1];
                gitHubClient = new GitHubClient(new ProductHeaderValue("Nuke"));
                var tokenAuth = new Credentials(GithubToken);
                gitHubClient.Credentials = tokenAuth;
            }
        });

    /// <summary>
    /// Adds a git tag if appropriate.
    /// </summary>
    Target TagRelease => _ => _
        .OnlyWhenDynamic(() => repositoryBranch == mainBranch || repositoryBranch.StartsWith(releaseBranchPrefix))
        .OnlyWhenDynamic(() => !string.IsNullOrWhiteSpace(GithubToken))
        .DependsOn(SetupGitHubClient)
        .DependsOn(SetBranch)
        .Executes(() =>
        {
            var version = repositoryBranch == "master" ? GitVersion.MajorMinorPatch : GitVersion.SemVer;
            GitLogger = (type, output) => Serilog.Log.Information(output);
            Git($"tag v{version}");
            Git($"push --tags");
        });

    /// <summary>
    /// Package the module.
    /// </summary>
    Target Package => _ => _
        .DependsOn(Clean)
        .DependsOn(SetManifestVersions)
        .DependsOn(Compile)
        .DependsOn(SetBranch)
        .DependsOn(TagRelease)
        .Executes(() =>
        {
            var stagingDirectory = ArtifactsDirectory / "staging";
            EnsureCleanDirectory(stagingDirectory);

            // PB Resources
            var pbResourcesDirectory = stagingDirectory / "pbResources";
            EnsureCleanDirectory(pbResourcesDirectory);
            CopyDirectoryRecursively(RootDirectory / "app", pbResourcesDirectory / "app");
            CopyDirectoryRecursively(RootDirectory / "css", pbResourcesDirectory / "css");
            CopyDirectoryRecursively(RootDirectory / "plugins", pbResourcesDirectory / "plugins");
            CopyDirectoryRecursively(RootDirectory / "scripts", pbResourcesDirectory / "scripts");
            var resourceFiles = GlobFiles(RootDirectory, "*.ascx", "*.html");
            resourceFiles.ForEach(f => {
                if (!f.Contains("License.html"))
                {
                    CopyFileToDirectory(f, pbResourcesDirectory);
                }
            });
            Compress(pbResourcesDirectory, stagingDirectory / "pbResources.zip");
            DeleteDirectory(pbResourcesDirectory);

            // Symbols
            var symbolFiles = GlobFiles(RootDirectory, "bin/Release/**/*.pdb");
            Helpers.AddFilesToZip(stagingDirectory / "symbols.zip", symbolFiles.ToArray());

            // Install files
            var installFiles = GlobFiles(RootDirectory, "manifest.dnn", "License.html");
            installFiles.ForEach(file => CopyFileToDirectory(file, stagingDirectory));

            // Sql Scripts
            var sqlScripts = GlobFiles(RootDirectory / "DBScripts", "*");
            sqlScripts.ForEach(file => CopyFileToDirectory(file, stagingDirectory / "DBScripts"));

            // Libraries - Copies all referenced libraries in the manifest.
            var manifest = GlobFiles(RootDirectory, "*.dnn").FirstOrDefault();
            var assemblies = GlobFiles(RootDirectory / "bin" / Configuration, "*.dll");
            var manifestAssemblies = Helpers.GetAssembliesFromManifest(manifest);
            assemblies.ForEach(assembly =>
            {
                var assemblyFile = new FileInfo(assembly);
                var assemblyIncludedInManifest = manifestAssemblies.Any(a => a == assemblyFile.Name);

                if (assemblyIncludedInManifest)
                {
                    CopyFileToDirectory(assembly, stagingDirectory / "bin", FileExistsPolicy.Overwrite);
                }
            });

            // Install package
            string fileName = moduleName + "_";
            if (GitVersion != null)
            {
                fileName += repositoryBranch == mainBranch ? GitVersion.MajorMinorPatch : GitVersion.SemVer;
            }
            fileName += "_install.zip";
            ZipFile.CreateFromDirectory(stagingDirectory, ArtifactsDirectory / fileName);
            DeleteDirectory(stagingDirectory);

            // Checksums
            var artifact = ArtifactsDirectory / fileName;
            string hash;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(artifact))
                {
                    var hashBytes = md5.ComputeHash(stream);
                    hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                }
            }

            var hashMd = new StringBuilder();
            hashMd.AppendLine($"## MD5 Checksums");
            hashMd.AppendLine($"| File       | Checksum |");
            hashMd.AppendLine($"|------------|----------|");
            hashMd.AppendLine($"| {fileName} | {hash}   |");
            hashMd.AppendLine();
            File.WriteAllText(ArtifactsDirectory / "checksums.md", hashMd.ToString());

            if (rootIsInDnn)
            {
                var installDir = RootDirectory.Parent.Parent.Parent.Parent.Parent / "Install" / "Module";
                var previousPackages = GlobFiles(installDir, $"*{moduleName}*");
                foreach (var previousPackage in previousPackages)
                {
                    DeleteFile(previousPackage);
                }
                CopyFileToDirectory(
                    ArtifactsDirectory / fileName,
                    installDir,
                    policy: FileExistsPolicy.Overwrite);
            }

            Serilog.Log.Information("Packaging succeeded!");
        });

    Target DeployBinaries => _ => _
        .OnlyWhenDynamic(() => rootIsInDnn)
        .DependsOn(Compile)
        .Executes(() =>
        {
            var dnnBinDirectory = RootDirectory.Parent.Parent.Parent.Parent.Parent / "bin";
            var manifest = GlobFiles(RootDirectory, "*.dnn").FirstOrDefault();
            var assemblyFiles = Helpers.GetAssembliesFromManifest(manifest);
            var files = GlobFiles(RootDirectory, "bin/Debug/*.dll", "bin/Debug/*.pdb", "bin/Debug/*.xml");
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                if (assemblyFiles.Contains(fileInfo.Name))
                {
                    Helpers.CopyFileToDirectoryIfChanged(file, dnnBinDirectory);
                }
            }
        });

    Target InstallNpmPackages => _ => _
       .Executes(() =>
       {
           NpmLogger = (type, output) =>
           {
               if (type == OutputType.Std)
               {
                   Serilog.Log.Information(output);
               }
               if (type == OutputType.Err)
               {
                   if (output.StartsWith("npm WARN", StringComparison.OrdinalIgnoreCase))
                   {
                       Serilog.Log.Warning(output);
                   }
                   else
                   {
                       Serilog.Log.Error(output);
                   }
               }
           };
           NpmInstall(s =>
               s.SetProcessWorkingDirectory(webProjectDirectory));
       });

    Target SetPackagesVersions => _ => _
        .DependsOn(TagRelease)
        .DependsOn(SetBranch)
        .Executes(() =>
        {
            if (GitVersion != null)
            {
                Npm($"version --no-git-tag-version --allow-same-version {GitVersion.MajorMinorPatch}", webProjectDirectory);
            }
        });

    Target BuildFrontEnd => _ => _
        .DependsOn(InstallNpmPackages)
        .DependsOn(SetManifestVersions)
        .DependsOn(TagRelease)
        .DependsOn(SetPackagesVersions)
        .Executes(() =>
        {
            NpmRun(s => s
                .SetProcessWorkingDirectory(webProjectDirectory)
                .AddArguments("build")
            );
        });

    Target DeployFrontEnd => _ => _
        .DependsOn(BuildFrontEnd)
        .Executes(() =>
        {
            var scriptsDestination = RootDirectory / "resources" / "scripts" / "structured-content";
            EnsureCleanDirectory(scriptsDestination);
            CopyDirectoryRecursively(RootDirectory / "module.web" / "dist" / "structured-content", scriptsDestination, DirectoryExistsPolicy.Merge);
        });

    Target DeployAll => _ => _
        .DependsOn(DeployFrontEnd)
        .DependsOn(DeployBinaries)
        .Executes(() =>
        {
        });

    Target LogInfo => _ => _
        .Before(Release)
        .DependsOn(TagRelease)
        .DependsOn(SetBranch)
        .Executes(() =>
        {
            Serilog.Log.Information($"Original branch name is {GitRepository.Branch}");
            Serilog.Log.Information($"We are on branch {repositoryBranch} and IsOnMasterBranch is {GitRepository.IsOnMasterBranch()} and the version will be {GitVersion.SemVer}");
            Serilog.Log.Information(SerializationTasks.JsonSerialize(GitVersion));
        });

    Target GenerateReleaseNotes => _ => _
        .OnlyWhenDynamic(() => repositoryBranch == mainBranch || repositoryBranch.StartsWith("release"))
        .OnlyWhenDynamic(() => !string.IsNullOrWhiteSpace(GithubToken))
        .DependsOn(Package)
        .DependsOn(SetupGitHubClient)
        .DependsOn(TagRelease)
        .DependsOn(SetBranch)
        .Executes(() =>
        {
            // Get the milestone
            var milestone = gitHubClient.Issue.Milestone.GetAllForRepository(repositoryOwner, repositoryName).Result.Where(m => m.Title == GitVersion.MajorMinorPatch).FirstOrDefault();
            if (milestone == null)
            {
                Serilog.Log.Warning("Milestone not found for this version");
                releaseNotes = "No release notes for this version.";
                return;
            }

            Serilog.Log.Information($"Found {milestone.Title} milestone.");

            // Get the PRs
            var prRequest = new PullRequestRequest()
            {
                State = ItemStateFilter.All
            };
            var pullRequests = gitHubClient.Repository.PullRequest.GetAllForRepository(repositoryOwner, repositoryName, prRequest).Result.Where(p =>
                p.Milestone?.Title == milestone.Title &&
                p.Merged == true &&
                p.Milestone?.Title == GitVersion.MajorMinorPatch);
            Serilog.Log.Information($"Found {pullRequests.Count()} pull requests with that milestone.");

            // Build release notes
            var releaseNotesBuilder = new StringBuilder();
            releaseNotesBuilder.AppendLine($"# {repositoryName} {milestone.Title}")
                .AppendLine("")
                .AppendLine($"A total of {pullRequests.Count()} pull requests where merged in this release.").AppendLine();

            foreach (var group in pullRequests.GroupBy(p => p.Labels[0]?.Name, (label, prs) => new { label, prs }))
            {
                releaseNotesBuilder.AppendLine($"## {group.label}");
                foreach (var pr in group.prs)
                {
                    releaseNotesBuilder.AppendLine($"- #{pr.Number} {pr.Title}. Thanks @{pr.User.Login}");
                }
            }

            // Checksums
            releaseNotesBuilder
                .AppendLine()
                .Append(File.ReadAllText(ArtifactsDirectory / "checksums.md"));

            releaseNotes = releaseNotesBuilder.ToString();
            Serilog.Log.Information(releaseNotes);
        });

    Target Release => _ => _
        .OnlyWhenDynamic(() => repositoryBranch == mainBranch || repositoryBranch.StartsWith(releaseBranchPrefix))
        .OnlyWhenDynamic(() => !string.IsNullOrWhiteSpace(GithubToken))
        .DependsOn(SetBranch)
        .DependsOn(SetupGitHubClient)
        .DependsOn(GenerateReleaseNotes)
        .DependsOn(TagRelease)
        .DependsOn(Package)
        .Executes(() =>
        {
            var newRelease = new NewRelease(repositoryBranch == mainBranch ? $"v{GitVersion.MajorMinorPatch}" : $"v{GitVersion.SemVer}")
            {
                Body = releaseNotes,
                Draft = true,
                Name = repositoryBranch == mainBranch ? $"v{GitVersion.MajorMinorPatch}" : $"v{GitVersion.SemVer}",
                TargetCommitish = GitVersion.Sha,
                Prerelease = repositoryBranch.StartsWith("release")
            };
            release = gitHubClient.Repository.Release.Create(repositoryOwner, repositoryName, newRelease).Result;
            Serilog.Log.Information($"{release.Name} released !");

            var artifactFile = GlobFiles(RootDirectory, "artifacts/**/*.zip").FirstOrDefault();
            var artifact = File.OpenRead(artifactFile);
            var artifactInfo = new FileInfo(artifactFile);
            var assetUpload = new ReleaseAssetUpload()
            {
                FileName = artifactInfo.Name,
                ContentType = "application/zip",
                RawData = artifact
            };
            var asset = gitHubClient.Repository.Release.UploadAsset(release, assetUpload).Result;
            Serilog.Log.Information($"Asset {asset.Name} published at {asset.BrowserDownloadUrl}");
        });

    Target CI => _ => _
        .DependsOn(LogInfo)
        .DependsOn(Package)
        .DependsOn(GenerateReleaseNotes)
        .DependsOn(TagRelease)
        .DependsOn(Release)
        .Executes(() =>
        {
        });
}
