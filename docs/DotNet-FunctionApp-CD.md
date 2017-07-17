Here is one example to Release a .NET method to an Azure Function App. You could adapt it with your own context, needs and constraints.

# Import the Release Definition

You could import [the associated Release Definition stored in this repository](/vsts/DotNet-FunctionApp-CD.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Release Definition

## Overview

![Release Overview](/docs/imgs/DotNet-FunctionApp-CD.PNG)

## Staging Environment

![Staging Release Overview](/docs/imgs/DotNet-FunctionApp-CD-Staging.PNG)

### Deployment conditions

- Trigger = After release creation

### Approvals

- Pre-deployment approver = Automatic
- Post-deployment approver = Automatic

### Variables

- ResourceGroupName = set appropriate
- SlotName = staging
- Location = East US

### Steps 

- Ensure Production Function App exists
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure Subscription = set appropriate
  - Action = Create Or Update Resource Group
  - Resource Group = $(ResourceGroupName)
  - Location = $(Location)
  - Template location = Linked artifact
  - Template = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/infra/[deploy.json](../infra/templates/deploy.json)
  - Override Template Parameters = -functionAppName $(ResourceGroupName)
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
  - Override Template Parameters = -functionAppName $(ResourceGroupName) -slotName $(SlotName)
  - Deployment Mode = Incremental
- Deploy Function App on Staging
  - Type = Deploy Staging
  - Version = 3.*
  - Azure Subscription = set appropriate
  - App Service Name = $(ResourceGroupName)
  - Deploy to Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotName)
  - Package or Folder = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/function
  - Publish using Web Deploy = true
  - Take App Offline = true
- Run UnitTests
  - Type = Visual Studio Test
  - Version = 2.*
  - Select tests using = Test assemblies
  - Test assemblies = \**\$(BuildConfiguration)\*IntegrationTests.dll\n!**\obj\**
  - Search folder = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/integration-tests
  - Test filter criteria = TestCategory=IntegrationTests
  - Build Platform = $(ReleasePlatform)
  - Build Configuration = $(ReleaseConfiguration)

## Production Environment

![Production Release Overview](/docs/imgs/DotNet-FunctionApp-CD-Production.PNG)

### Deployment conditions

- Trigger = After successful deployment to another environment ("Staging")

### Approvals

- Pre-deployment approver = Specific Users (set appropriate users)
- Post-deployment approver = Automatic

### Variables

- ResourceGroupName = set appropriate
- SlotToSwap = staging
- Location = East US

### Steps

- Swap Staging to Production
  - Type = Azure App Service Manage (PREVIEW)
  - Version = 0.*
  - Azure Subscription = set appropriate
  - Action = Swap Slots
  - App Service Name = $(ResourceGroupName)
  - Resource Group = $(ResourceGroupName)
  - Source Slot = $(SlotToSwap)
  - Swap with Production = true
- Set Resource Group Lock
  - Type = Azure PowerShell
  - Version = 1.*
  - Azure Connection Type = Azure Resource Manager
  - Azure Subscription = set appropriate
  - Script Type = Script File Path
  - Script Path = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/scripts/[AddResourceGroupLock.ps1](../infra/scripts/AddResourceGroupLock.ps1)
  - Script Arguments = $(ResourceGroupName)
- Check Production URL
  - Type = [Check URL Status](https://marketplace.visualstudio.com/items?itemName=saeidbabaei.checkUrl)
  - URL = https://$(ResourceGroupName).azurewebsites.net/api/SampleHelloDotNetFunction/test