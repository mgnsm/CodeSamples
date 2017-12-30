# A Minimal UWP App Built Using The Windows Runtime C++ Template Library (WRL)

An example of a minimal UWP app that displays a window with a `TextBlock` without using any language projections.

You can compile the code using the `cl.exe` compiler from the command line of a Visual Studio command prompt:

`>cd "c:\Repositories\CodeSamples\MinimalWRLSampleApp\`

`>cl.exe /D WINAPI_FAMILY=WINAPI_FAMILY_APP Program.cpp Source.cpp -link RuntimeObject.lib /APPCONTAINER /WINMD:NO /OUT:MinimalWRLSampleApp.exe`

`>powershell add-appxpackage –register AppxManifest.xml`
