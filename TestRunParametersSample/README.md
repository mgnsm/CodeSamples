# Override TestRunParameters in .NET Core

An example of how to replace a `TestRunParameter` defined in a source controlled `.runsettings` file with a secret parameter defined in the Azure DevOps portal. 

The YAML defined build pipeline creates a new temporary `.runsettings` file using Powershell and passes it to the `dotnet test` command using the `--settings` (`-s`) option.

Please refer to [this](https://magnusmontin.wordpress.com/2019/05/30/override-testrunparameters-in-net-core/) blog post for more information.