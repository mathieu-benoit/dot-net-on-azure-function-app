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
- Deploy Web App
  - Type = Azure App Service Deploy
  - Version = 3.*
  - Azure Subscription = set appropriate
  - App Service Name = $(ResourceGroupName)
  - Deploy to Slot = true
  - Resource Group = $(ResourceGroupName)
  - Slot = $(SlotName)
  - Package or Folder = $(System.DefaultWorkingDirectory)/DotNet-FunctionApp-CI/drop
  - Publish using Web Deploy = true
  - Take App Offline = true

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
  - Azure Connection Type = set appropriate
  - Azure RM Subscription = set appropriate
  - Script Path = $(System.DefaultWorkingDirectory)/AspDotNetCore-AppServiceWindows-CI/scripts/[AddResourceGroupLock.ps1](../infra/scripts/AddResourceGroupLock.ps1)
  - Script Arguments = $(ResourceGroupName)