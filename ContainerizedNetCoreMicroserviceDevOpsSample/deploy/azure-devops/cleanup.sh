#!/bin/bash

PrintError()
{
	red='\033[1;31m'
	noColor='\033[0m'
	echo "${red}Error: $1 ${noColor}"
}

if [ $# -eq 2 ]; then
	organization=$1
	teamProjectName=$2
else
	PrintError 'Invalid number of arguments. Should be 2.'
	exit 1
fi

# Set the default Azure DevOps organization
az devops configure --defaults organization=https://dev.azure.com/$organization

teamProjectId=$(az devops project list --query "value[?name=='$teamProjectName'].id" -o tsv)
if [ -z $teamProjectId ]; then 
	PrintError "Team project 'teamProjectName' not found."
	exit 2
fi

echo "Deleting team project named '$teamProjectName'..."
az devops project delete --id $teamProjectId --yes

echo "Deleting Azure service principals..."
az ad sp delete --id 'http://sp-owner-containerized-microservice-sample'
az ad sp delete --id 'http://sp-acrpull-containerized-microservice-sample'

exit 0