# Deploy to Azure DevOps
To be able to use [this Bash script](../deploy/azure-devops/create-pipelines.sh) to deploy the build and release sample YAML pipelines to a team project in Azure DevOps, the following prerequisites are required:
 
- An Azure subscription
- An Azure DevOps organization
- An up-to-date version of the [Azure CLI](https://docs.microsoft.com/sv-se/cli/azure/install-azure-cli?WT.mc_id=AZ-MVP-5001077)
- An up-to-date version of [Docker Desktop for Mac or Windows](https://www.docker.com/products/docker-desktop) or the [Docker Compose CLI for Linux](https://docs.docker.com/engine/context/aci-integration/#install-the-docker-compose-cli-on-linux)
- Git
- A GitHub account
 
1. [Create a new repository on GitHub](https://docs.github.com/en/free-pro-team@latest/github/creating-cloning-and-archiving-repositories/creating-a-new-repository)
 
2. [Clone](https://docs.github.com/en/free-pro-team@latest/github/creating-cloning-and-archiving-repositories/cloning-a-repository) this repository to a local folder on your development machine:
 
        git clone https://github.com/mgnsm/CodeSamples.git
        cd CodeSamples
 
3. Push the cloned sample code to your newly created repository:
 
        git remote set-url origin https://github.com/<your-username>/<your-repository>.git
        git push -u origin master

4. Create a `development` branch and push it as well:

       git checkout -b development
       git push -u origin development
 
5. [Create a GitHub personal access token (PAT)](https://docs.github.com/en/free-pro-team@latest/github/authenticating-to-github/creating-a-personal-access-token). Azure DevOps will use this for authentication to GitHub.
 
6. Install the [Azure CLI](https://docs.microsoft.com/sv-se/cli/azure/install-azure-cli?WT.mc_id=AZ-MVP-5001077)
 
7. Install the [Azure DevOps Extension](https://github.com/Azure/azure-devops-cli-extension) for the Azure CLI:
 
        az extension add --name azure-devops
 
8. Run the `az login` command in a Bash shell to [sign in to Azure interactively](https://docs.microsoft.com/en-us/cli/azure/authenticate-azure-cli?WT.mc_id=AZ-MVP-5001077)
 
9. Run the [script](../deploy/azure-devops/create-pipelines.sh) to create an Azure DevOps team project and set up the build and release pipelines. The Bash script requires the following parameters in the listed order:
 
    - The name of your Azure DevOps organization
    - The name of the Azure DevOps project to be created if it doesn't already exist
    - Your GitHub username
    - The name of your GitHub repository
    - Your GitHub PAT
    - The name of the Azure WebApp service (optional)
    - The name of the ACR (optional)
 
Here is an example of how to run the script from a Bash shell:

    sh deploy/azure-pipelines/create-pipelines.sh \
      'contoso' \
      'sampleproject' \
      'mgnsm' \
      'CodeSamples' \
      '16216f467bbfe00f5f49c9f02a4266a15f9421dd' \
      'app-containerized-microservice' \
      'mydemoacr'

If you don't supply any names for the WebApp and ACR parameters, the script will generate some random names for these Azure resources. All other parameters are mandatory.
 
The script does the following:
 
- Checks whether a team project with the name specified by the first parameter already exists. If it doesn't, the script creates a new team project using the `az devops project create` command.
- Creates a resource group to hold the Azure resources.
- Checks whether an Azure Resource Manager service connection with the name "AzureServiceConnection" already exists in the team project. If it doesn't, it's created along with an Azure service principal that has full access to manage resources, including the ability to assign roles to them within the previously created resource group. This is required for the service principal used in the CI pipeline to be able to assign the `AcrPush` role to the ACR.
- Checks whether a GitHub service connection with the name "GitHubServiceConnection" already exists and creates it using the `az devops service-endpoint github create` command if it doesn't.
- Creates another Azure service principal to be used by the WebApp and assigns it the `AcrPush` role.
- Creates the [CI pipeline](../build/ci-pipeline.yml) (assuming there is no existing pipeline named "CI-Pipeline" in the team project).
- Sets the 'azureContainerRegistryName' variable that is used in the YAML file to the name of the ACR as specified by the script's last parameter.
- Sets the 'servicePrincipalObjectId' variable, that is also referred to in the YAML file, to the object id of the service principal that the app service will use to authenticate against the ACR.
- Creates the [CD pipeline](../release/cd-pipeline.yml) (assuming there is no existing pipeline named "CD-Pipeline" in the team project).
- Sets the "azureContainerRegistryName" and "azureWebAppName" variables that are used in the YAML file to the values of the last and second last script parameters respectively.
- Sets the "servicePrincipalAppId" and "servicePrincipalPassword" variables that are required for the WebApp created by the [ARM template](../deploy/azure/webapp/azuredeploy.json) to be able to authenticate against the ACR. The latter is defined as a [secret variable](https://docs.microsoft.com/en-us/azure/devops/pipelines/process/variables?WT.mc_id=AZ-MVP-5001077&view=azure-devops&tabs=yaml%2Cbatch#secret-variables).
 
Once the script has finished, you should have a team project with two pipelines set up. You can then run (queue) a build of the `master` branch using the following command:
 
    az pipelines run --branch master --name CI-Pipeline --project <name-your-team-project>

This is the equivalent of starting a manual release to the production environment as a succeeded build of any of the `master` or `development` branches will trigger the CD pipeline. 

The CI (build) pipeline will be triggered automatically if you submit a pull-request (PR) against the `master` or `development` branch, and when the PR has been approved and merged into any of these branches.

The CD (release) pipeline will be triggered automatically once the PR has been merged. Depending on the branch (`master` or `development`), the build will be released to the production or QA environment. 

An environment is a publicly available cloud-hosted Azure Web app service in this case. The URL to the published app is determined by the value of the 'azureWebAppName' CD pipeline parameter. If it's set to "app-containerized-microservice", for example, the production app will be available at http://app-containerized-microservice.azurewebsites.net and the QA version at http://app-containerized-microservice-qa.azurewebsites.net. The CD pipeline appends "-qa" to the name specified in the variable based on the source branch that triggered the release and the value of the 'environment´ pipeline parameter.

You can queue a release manually from the Azure DevOps portal. There is a [feature request](https://github.com/Azure/azure-devops-cli-extension/issues/972) to support passing parameters to a pipeline using the `az pipelines run`.

### Cleanup

There is another [cleanup script](../deploy/azure-devops/cleanup.sh) that you can use to remove the Azure DevOps team project and the Azure service principals that the [create-pipelines.sh](../deploy/azure-devops/cleanup.sh) script creates. It takes two parameters: the name of the Azure DevOps organization and the name of the team project, in that order:

    sh cleanup.sh contoso sampleproject

If you have run any of the pipelines, you may also want to remove the resource group that holds all the Azure resources:

    az group delete --name rg-containerized-microservice-sample --yes
