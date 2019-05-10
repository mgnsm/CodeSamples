# Protocol Activation and Redirection In Packaged Apps

An example of how to handle Uniform Resource Identifier (URI) protocol and file extension activation in an MSIX or desktop bridge packaged WPF application.

The Visual Studio solution contains a [Windows Application Packaging Project](https://docs.microsoft.com/en-us/windows/uwp/porting/desktop-to-uwp-packaging-dot-net) and two sample WPF applications that target the .NET Framework 4.6.1. The `SingleInstanceApp` handles all activations in the same instance and the `MultipleInstancesSampleApp` handles a single activation per instance, i.e. a new instance of the application is created for each activation.

Please refer to this [this](https://blog.magnusmontin.net/2019/05/10/handle-protocol-activation-and-redirection-in-packaged-apps/) blog post for more information.
