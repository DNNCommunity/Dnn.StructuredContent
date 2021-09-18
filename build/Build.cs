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
using System.Xml;
using System.Globalization;
using Octokit;
using BuildHelpers;
using System.IO.Compression;
using System.IO;

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

    GitHubClient gitHubClient;

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
            Logger.Info($"Set branch name to {repositoryBranch}");
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
                .SetProjectFile(Solution));
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
                        Logger.Normal($"Found package {package.Attributes["name"].Value} with version {version.Value}");
                        version.Value = $"{GitVersion.Major.ToString("00", CultureInfo.InvariantCulture)}.{GitVersion.Minor.ToString("00", CultureInfo.InvariantCulture)}.{GitVersion.Patch.ToString("00", CultureInfo.InvariantCulture)}";
                        Logger.Normal($"Updated package {package.Attributes["name"].Value} to version {version.Value}");
                    }
                }
                doc.Save(manifest);
                Logger.Normal($"Saved {manifest}");
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
            Logger.Info($"We are on branch {repositoryBranch}");
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
            GitLogger = (type, output) => Logger.Info(output);
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

            if (RootDirectory.Parent.Parent.ToString().EndsWith("Dnn.PersonaBar", StringComparison.OrdinalIgnoreCase))
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

            Logger.Success("Packaging succeeded!");
        });

}
