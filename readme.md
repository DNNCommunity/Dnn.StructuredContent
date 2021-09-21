# DNN Structured Content
This is a proof of concept for a structured content solution for DNN Platform.

This should not be used in a production environment. Anything can change at this stage of development.

## Building
- Install a local DNN Website of the current latest version (we are not planning to support old DNN versions)
- Clone this repository into the `DesktopModules\Admin\Dnn.PersonaBar\Modules\` folder of that website
- Open Visual Studio as an administrator, then open the `Dnn.StructuredContent.sln` solution.
- Hit the `Package` button in Visual Studio to start a build.
- Once the build is done, go to that DNN instance to `Extensions` and then `Available extensions` and you should be able to install the module on the instance.
- To debug after that initial installation, use the `Deploy ...` Options in that same dropdown you used `Package` and then attach the debugger to the website.

## Releasing
The project includes some nice build/release automations. In order to create a new release:
- Never commit directly to the `main` branch, this is reserved for official published releases
- Plan your versions ahead using milestones, only merge into the `develop` branch the things you plan on publishing on the next version and assign that mileston to each PR.
- When ready to release, create a `release/x.x.x` where x.x.x is the version you want to release (must match some milestones of merged PRs)
- Withing a few minutes, you will see an unpublished release with that version that will be named x.x.x-beta1, it will have the build artifact as well as release notes based on PRs that got merged for that milestone, you can publish this for testing.
- When testing is done and you are ready for an official release, merge that `release/x.x.x` branch into the `main` branch, within a few minutes a new "non-beta" unpublished release will be ready for you to adjust/publish.
