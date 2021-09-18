# DNN Structured Content
This is a proof of concept for a structured content solution for DNN Platform.

This should not be used in a production environment. Anything can change at this stage of development.

## Building
- Install a local DNN Website of the current latest version (we are not planning to support old DNN versions)
- Clone this repository into the `DesktopModules\Admin\Dnn.PersonaBar\Modules\` folder of that website
- Open Visual Studio as an administrator, then open the `Dnn.StructuredContent.sln` solution.
- Hit the `Package` button in Visual Studio to start a build.
- Once the build is done, go to that DNN instance to `Extensions` and then `Available extensions` and you should be able to install the module on the instance.
- To debug after that initial installation, use the `Deploy ...` options in that same dropdown you used `Package` and then attach the debugger to the website.