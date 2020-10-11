# Continuous Integration and Deployment of a Containerized .NET Core Microservice using Azure DevOps

An example that demonstrates how to use Azure Pipelines to set up continuous integration (CI) and deployment (CD) for a containerized .NET Core microservice that runs in Azure and how to automate the creation process using the Azure CLI.
 
The solution is cloud-native, cross-platform and powered by .NET Core, Docker, Git, GitHub, Azure and Azure DevOps. It uses Azure service principals and GitHub personal access tokens (PATs) for authentication and the Azure CLI and Bash for automation.
 
## Folder Structure
- The **src** folder contains the source code for the (in this case rather simple) ASP.NET core based microservice app, including the Visual Studio solution file (`.sln`) and all tests.
 
- The **build** folder contains a YAML based CI pipeline that builds and tests the microservice in a Docker container before it pushes a versioned Docker image to a private Azure Container Registry (ACR) in the cloud that is created on demand during the build.
 
- The **release** folder contains a CD pipeline that deploys an Azure WebApp for Containers. The app service pulls the container image from the ACR and spins up a container that runs the microservice in the cloud. When releasing to production, it also uses the [GitHub Release task](https://docs.microsoft.com/en-us/azure/devops/pipelines/tasks/utility/github-release?WT.mc_id=AZ-MVP-5001077) to create a draft of a GitHub release.
 
- The **deploy** folder contains the resource manager (ARM) templates that are used to create the Azure resources. It also contains a Bash script that can be used to automate the creation of the pipelines and the Azure DevOps project that hosts them, including all required service connections and variables.
 
- The **docs** folder contains instructions of how to use the script in [deploy](deploy), and what it does, to set up the pipelines after you have cloned and pushed the sample code to a git repository of your own that you have sufficient permissions to access.
 
## DevOps Scenario
 
### Branching Model
 
The branching model to be used with this example consists of two main branches with an infinite lifetime that are supposed to be [protected](https://docs.github.com/en/enterprise-server@2.20/github/administering-a-repository/about-protected-branches):
 
- `master`
- `development`
 
When a pull-request (PR) from a supporting (hotfix, feature, release, etc.) branch is approved and merged into any of these two branches, the CI pipeline creates a Docker image, tags it with an automatically increased semantic version number that is unique per build and branch, and pushes it to the ACR.
 
Before a PR is merged, the code in the PR branch is validated (built and tested) using the .NET Core CLI in both Linux and Windows Docker containers and on macOS.
 
All of this is defined in the [CI pipeline YAML file](build/ci-pipeline.yml).
 
### Environments
 
The [CD pipeline YAML file](release/cd-pipeline.yml) defines two environments:
 
- QA
- Production
 
The source code in the `master` branch should always reflect a production-ready state of the app. When the `master` branch builds successfully, a production release is rolled-out to a Web App for Containers app service in Azure that represents the production environment.
 
`development` builds are continuously deployed to another app service that represents the QA environment and whose hostname in Azure is suffixed with "-qa", e.g. http://&lt;your-web-app-qa&gt;.azurewebsites.net.
 
If you want it any other way, like if you for example want to deploy another branch than `master` or `development` to any of the environments, there is an 'environment' parameter defined in the YAML pipeline that you can set manually when you run the pipeline from the Azure DevOps web portal. It is currently not supported to set pipeline parameters in the Azure CLI but there is [feature request](https://github.com/Azure/azure-devops-cli-extension/issues/972).
 
## Sample Usage
 
- [How to Clone and Deploy the Sample to Azure DevOps](docs/deploy-to-azure-devops.md)
- [How to Build and Run the Sample Code using Docker, the .NET Core CLI or Visual Studio](docs/build-and-run.md)
 
## Questions and Feedback
If you have any questions or feedback, please [open an issue](https://docs.github.com/en/free-pro-team@latest/github/managing-your-work-on-github/creating-an-issue).