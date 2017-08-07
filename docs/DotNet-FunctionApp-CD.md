Here is one example to Release a .NET method to an Azure Function App. You could adapt it with your own context, needs and constraints.

2 ways to create the associated Release Definition + another one just to deploy the Azure services:

- [Import the Release Definition](#import-the-release-definition)
- [Create manually the Release Definition](#create-manually-the-release-definition)
- [Deploy to Azure buttons](#deploy-to-azure-buttons)

# Import the Release Definition

You could import [the associated Release Definition stored in this repository](/vsts/DotNet-FunctionApp-CD.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Release Definition

## Overview

![Release Overview](/docs/imgs/DotNet-FunctionApp-CD.PNG)

## Artifacts

- Build - DotNet-FunctionApp-CI
  - Source (Build definition) = DotNet-FunctionApp-CI
  - Default version = Latest
  - Source alias = DotNet-FunctionApp-CI
  - Continuous deployment trigger = Enabled

## Variables

- ResourceGroupName = set appropriate
- SlotName = staging
- Location = East US
- ReleaseConfiguration = Release
- ReleasePlatform = Any CPU
- AppServiceUrl = 
  - Empty, because it will be set by the "Deploy Function App on Staging" task and will be consumed by the "Replace tokens in IntegrationTests config file" task.
- FunctionAppName = set appropriate
- FunctionName = SampleHelloDotNetFunction

![Release Variables](/docs/imgs/DotNet-FunctionApp-CD-Variables.PNG)

## Staging Environment

![Staging Release Overview](/docs/imgs/DotNet-FunctionApp-CD-Staging.PNG)

### Pre-deployment conditions

- Triggers
  - Select the source of the trigger = Release
- Pre-deployment approvers
  - Approval type = Automatic

### Tasks 

- Set Resource Group ResourceTypes policy
  - Type = Azure PowerShell
  - Version = 1.*
  - Azure Connection Type = Azure Resource Manager
  - Azure Subscription = set appropriate
  - Script Type = Script File Path
  - Script Path = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/scripts/[AddResourceGroupAllowedResourceTypesPolicy.ps1](../infra/scripts/AddResourceGroupAllowedResourceTypesPolicy.ps1)
  - Script Arguments = -ResourceGroupName $(ResourceGroupName)
- Set Resource Group Locations policy
  - Type = Azure PowerShell
  - Version = 1.*
  - Azure Connection Type = Azure Resource Manager
  - Azure Subscription = set appropriate
  - Script Type = Script File Path
  - Script Path = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/scripts/[AddResourceGroupAllowedLocationsPolicy.ps1](../infra/scripts/AddResourceGroupAllowedLocationsPolicy.ps1)
  - Script Arguments = -ResourceGroupName $(ResourceGroupName)
- Ensure Production Function App exists
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/infra/[deploy.json](../infra/templates/deploy.json)
  - Override Template Parameters = -functionAppName $(FunctionAppName)
  - Deployment Mode = Incremental
- Provision Staging
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/infra/[deplo-slot.json](../infra/templates/deploy-slot.json)
  - Override Template Parameters = -functionAppName $(FunctionAppName) -slotName $(SlotName)
  - Deployment Mode = Incremental
- Deploy Function App on Staging
  - Type = Azure App Service Deploy
  - Version = 3.*
  - Azure Subscription = set appropriate
  - App Service Name = $(FunctionAppName)
  - Deploy to Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotName)
  - Package or Folder = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/function
  - Publish using Web Deploy = true
  - App Service URL = BaseUrl
- Check Staging URL
  - Type = [Check URL Status](https://marketplace.visualstudio.com/items?itemName=saeidbabaei.checkUrl)
  - Version = 1.*
  - URL = https://$(FunctionAppName)-$(SlotName).azurewebsites.net/api/$(FunctionName)?name=test
- Replace tokens in IntegrationTests config file
  - Type = [Replace Tokens](https://marketplace.visualstudio.com/items?itemName=qetza.replacetokens)
  - Version = 2.*
  - Target files = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/integration-tests/*.config
  - Files encoding = auto
  - Write unicode BOM = true
  - Action (Missing variables) = fail
  - Keep token = true
  - Token prefix = #{
  - Token suffix = }#
- Run IntegrationTests
  - Type = Visual Studio Test
  - Version = 2.*
  - Select tests using = Test assemblies
  - Test assemblies = *IntegrationTests.dll
  - Search folder = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/integration-tests
  - Select test platform using = Version
  - Test paltform version = Latest
  - Test run title = IntegrationTests
  - Build Platform = $(ReleasePlatform)
  - Build Configuration = $(ReleaseConfiguration)
  - Upload test attachments = true

## Production Environment

![Production Release Overview](/docs/imgs/DotNet-FunctionApp-CD-Production.PNG)

### Pre-deployment conditions

- Triggers
  - Select the source of the trigger = Environment
  - Environment(s) that will trigger a deployment = Staging
- Pre-deployment approvers
  - Approval type = Specific users
  - Select approvers = set appropriate

### Tasks

- Swap Staging to Production
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Swap Slots
  - App Service Name = $(FunctionAppName)
  - Resource Group = $(ResourceGroupName)
  - Source Slot = $(SlotName)
  - Swap with Production = true
- Set Resource Group Lock
  - Type = Azure PowerShell
  - Version = 1.*
  - Azure Connection Type = Azure Resource Manager
  - Azure Subscription = set appropriate
  - Script Type = Script File Path
  - Script Path = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/scripts/[AddResourceGroupDoNotDeleteLock.ps1](../infra/scripts/AddResourceGroupDoNotDeleteLock.ps1)
  - Script Arguments = -ResourceGroupName $(ResourceGroupName)
- Check Production URL
  - Type = [Check URL Status](https://marketplace.visualstudio.com/items?itemName=saeidbabaei.checkUrl)
  - Version = 1.*
  - URL = https://$(FunctionAppName).azurewebsites.net/api/$(FunctionName)?name=test

### General remark

  For the "Set Resource Group Lock" step, you will need to make sure that your default Service Principal user created by VSTS (during the Azure RM service endpoint creation) has the Owner role and not by default the Contributor role. Otherwise this task will fail. To assign the Owner role, you could go to the Access control (IAM) blade of your Azure subscription within the new Azure portal and then Assign (Add button) the associated VisualStudioSPN... user to the Owner role.

## Rollback Environment

This environment should be used just if necessary when the bad things happened in Production just after this release pipeline. It allows to be prepared by automation to rollback the changed and get back to the previous version.

![Rollback Release Overview](/docs/imgs/DotNet-FunctionApp-CD-Rollback.PNG)

### Pre-deployment conditions

- Triggers
  - Select the source of the trigger = Environment
  - Environment(s) that will trigger a deployment = Production
- Pre-deployment approvers
  - Approval type = Specific users
  - Select approvers = set appropriate

### Tasks

- Rollback Swap
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Swap Slots
  - App Service Name = $(FunctionAppName)
  - Resource Group = $(ResourceGroupName)
  - Source Slot = $(SlotName)
  - Swap with Production = true
- Check Production URL
  - Type = [Check URL Status](https://marketplace.visualstudio.com/items?itemName=saeidbabaei.checkUrl)
  - Version = 1.*
  - URL = https://$(FunctionAppName).azurewebsites.net/api/$(FunctionName)?name=test

# Deploy to Azure buttons

By using the buttons below it's another way to deploy the Azure services without VSTS and without taking into account the app/method by itself, just deploying the infrastructure within Azure. 

The Azure Function App + its Blob storage + Application Insights:

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fdot-net-on-azure-function-app%2Fmaster%2Finfra%2Ftemplates%2Fdeploy.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>

The Azure Function App Slot + its Application Insights (you should deploy the Azure Function App before, see button above):

<a href="https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fmathieu-benoit%2Fdot-net-on-azure-function-app%2Fmaster%2Finfra%2Ftemplates%2Fdeploy-slot.json" target="_blank">![Deploy to Azure](http://azuredeploy.net/deploybutton.png)</a>