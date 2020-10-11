#!/bin/bash

CreatePipeline()
{
	if [ -z $(az pipelines list --project $teamProjectName --name $1 --top 1 --query [].id -o tsv) ]; then
		echo "Creating an Azure Pipeline named '$1'..."
		az pipelines create --name $1 \
		  --project $teamProjectName \
		  --repository "https://github.com/$gitHubUserName/$gitHubRepository" \
          --service-connection $gitHubServiceConnectionId \
          --yaml-path $2 \
          --skip-run
	else
		PrintWarning "Skipping creating Azure Pipeline '$1' as it already exists."
	fi
}

PrintWarning()
{
	yellow='\033[1;33m'
	noColor='\033[0m'
	echo "${yellow}WARNING: $1${noColor}"
}

SetPipelineVariable()
{
	if [ -z "$(az pipelines variable list --project $teamProjectName --pipeline-name $3 --query $1)" ]; then		
		echo "Sets the '$1' pipeline variable..."
		az pipelines variable create --project $teamProjectName  \
		  --pipeline-name $3 \
		  --name $1 \
		  --value "$2" \
		  --secret $4
	else
		PrintWarning "Skipping setting the '$1' pipeline variable as it is already set."
	fi
}

# Generate a unique string
uniqueName=$(env LC_CTYPE=C tr -dc 'a-z' < /dev/urandom | fold -w 32 | head -n 1)

if [ $# -eq 5 ]; then
	organization=$1
	teamProjectName=$2
	gitHubUserName=$3
	gitHubRepository=$4
	gitHubPat=$5
	azureWebAppName=$uniqueName
	azureContainerRegistryName=$uniqueName
elif [ $# -eq 6 ]; then
	organization=$1
	teamProjectName=$2
	gitHubUserName=$3
	gitHubRepository=$4
	gitHubPat=$5
	azureWebAppName=$6
	azureContainerRegistryName=$uniqueName
elif [ $# -eq 7 ]; then
	organization=$1
	teamProjectName=$2
	gitHubUserName=$3
	gitHubRepository=$4
	gitHubPat=$5
	azureWebAppName=$6
	azureContainerRegistryName=$7
else
	red='\033[1;31m'
	noColor='\033[0m'
	echo "${red}Error: Invalid number of arguments. Should be between 5 and 7.${noColor}"
	exit 1
fi

resourceGroupName='rg-containerized-microservice-sample'
azureLocation='westeurope'

# Set the default Azure DevOps organization
az devops configure --defaults organization=https://dev.azure.com/$organization

# Check whether the team project already exists
if [ -z "$(az devops project list --query "value[?name=='$teamProjectName'].name" -o tsv)" ]; then 
	# Create a new team project
	echo "Creating a new Azure DevOps team project named '$teamProjectName'..."
	az devops project create --name $teamProjectName
else
	PrintWarning "Skipping creating Azure DevOps team project '$teamProjectName' as it already exists."
fi

# Create a resource group to hold the Azure resources
if [ $(az group exists --name $resourceGroupName) = false ]; then
	echo "Creating an Azure resource group named '$resourceGroupName'..."
	az group create --name $resourceGroupName --location $azureLocation
else
	PrintWarning "Skipping creating Azure resource group '$resourceGroupName' as it already exists."
fi

# Get the object ID of the resource group
resourceGroupId=$(az group show --name $resourceGroupName --query id --output tsv)

#Check whether the Azure Resource Manager service connection already exists
azureServiceConnectionName='AzureServiceConnection'
azureServiceConnectionId=$(az devops service-endpoint list --project $teamProjectName --query "[?name=='$azureServiceConnectionName']".id -o tsv)
if [ -z $azureServiceConnectionId ]; then 
	# Create a service principal
	servicePrincipalName='http://sp-owner-containerized-microservice-sample'
	echo "Creating an Azure service principal named '$servicePrincipalName'..."
	password=$(az ad sp create-for-rbac --name $servicePrincipalName --role Owner --scope $resourceGroupId --query password --output tsv)
	appId=$(az ad sp show --id $servicePrincipalName --query appId --output tsv)
	tenant=$(az ad sp show --id $servicePrincipalName --query appOwnerTenantId --output tsv)
	subscriptionId=$(az account show --query id --output tsv)
	subscriptionName=$(az account show --query name --output tsv)

	# Create an Azure Resource Manager service connection
	echo "Creating an Azure Resource Manager service connection named '$azureServiceConnectionName'..."
	export AZURE_DEVOPS_EXT_AZURE_RM_SERVICE_PRINCIPAL_KEY=$password
	az devops service-endpoint azurerm create \
	  --azure-rm-service-principal-id $appId \
	  --azure-rm-subscription-id $subscriptionId \
	  --azure-rm-subscription-name "$subscriptionName" \
	  --azure-rm-tenant-id $tenant \
	  --name $azureServiceConnectionName  \
	  --project $teamProjectName
	
	azureServiceConnectionId=$(az devops service-endpoint list --project $teamProjectName --query "[?name=='$azureServiceConnectionName']".id -o tsv)
else
	PrintWarning "Skipping creating Azure Resource Manager service connection '$azureServiceConnectionName' as it already exists."
fi

# Allow all pipelines to access the Azure Resource Manager service connection
az devops service-endpoint update --id $azureServiceConnectionId  \
  --enable-for-all  \
  --project $teamProjectName

# Check whether the GitHub service connection already exists
gitHubServiceConnectionName='GitHubServiceConnection'
gitHubServiceConnectionId=$(az devops service-endpoint list --project $teamProjectName --query "[?name=='$gitHubServiceConnectionName']".id -o tsv)
if [ -z $gitHubServiceConnectionId ]; then 
	# Create a GitHub service connection
	echo "Creating a GitHub service connection named '$gitHubServiceConnectionName'..."
	export AZURE_DEVOPS_EXT_GITHUB_PAT=$gitHubPat
	az devops service-endpoint github create \
	  --github-url https://github.com \
	  --name $gitHubServiceConnectionName  \
	  --project $teamProjectName
	gitHubServiceConnectionId=$(az devops service-endpoint list --project $teamProjectName --query "[?name=='$gitHubServiceConnectionName']".id -o tsv)
else
	PrintWarning "Skipping creating GitHub service connection '$gitHubServiceConnectionName' as it already exists."
fi

# Allow all pipelines to access the GitHub service connection
az devops service-endpoint update --id $gitHubServiceConnectionId  \
  --enable-for-all  \
  --project $teamProjectName
  
# Create an Azure Service Principal for accessing the Azure Container Registry (ACR)
acrServicePrincipalName='http://sp-acrpull-containerized-microservice-sample'
echo "Creating an Azure service principal named '$acrServicePrincipalName'..."
acrServicePrincipalPassword=$(az ad sp create-for-rbac --name $acrServicePrincipalName --role AcrPull --scope $resourceGroupId --query password --output tsv)
acrServicePrincipalAppId=$(az ad sp show --id $acrServicePrincipalName --query appId --output tsv)
acrServicePrincipalObjectId=$(az ad sp show --id $acrServicePrincipalAppId --query objectId --output tsv)

# Create the CI pipeline
ciPipelineName='CI-Pipeline'
CreatePipeline $ciPipelineName 'ContainerizedNetCoreMicroserviceDevOpsSample\build\ci-pipeline.yml'
SetPipelineVariable 'azureContainerRegistryName' $azureContainerRegistryName $ciPipelineName false
SetPipelineVariable 'servicePrincipalObjectId' $acrServicePrincipalObjectId $ciPipelineName false

# Create the CD pipeline
cdPipelineName='CD-Pipeline'
CreatePipeline $cdPipelineName 'ContainerizedNetCoreMicroserviceDevOpsSample\release\cd-pipeline.yml'
SetPipelineVariable 'azureContainerRegistryName' $azureContainerRegistryName $cdPipelineName false
SetPipelineVariable 'azureWebAppName' $azureWebAppName $cdPipelineName false
SetPipelineVariable 'servicePrincipalAppId' $acrServicePrincipalAppId $cdPipelineName false
SetPipelineVariable 'servicePrincipalPassword' "$acrServicePrincipalPassword" $cdPipelineName true

exit 0