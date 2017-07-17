Here is one example to Build a .NET method to be deployed as an Azure Function App via VSTS. You could adapt it with your own context, needs and constraints.

![Build Overview](/docs/imgs/DotNet-FunctionApp-CI.PNG)

# Import the Build Definition

You could import [the associated Build Definition stored in this repository](/vsts/DotNet-FunctionApp-CI.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Build Definition

## Variables

- BuildConfiguration = release
- ValidateTemplatesResourceGroup = validate-templates-rg

## Repository

- Repository Type = GitHub
- Connection = set appropriate
- Repository = mathieu-benoit/dot-net-on-azure-function-app
- Default branch = master

## Triggers

- Continuous Integration (CI) = true

## Process - Build process

- Name = DotNet-FunctionApp-CI
- Default agent queue = Hosted VS2017

## Steps 

- NuGet restore
  - Type = NuGet Installer
  - Version = 0.*
  - Path to solution or packages.config = **\*.sln
  - Installation type = Restore
  - NuGet Version (Advanced) = 4.0.0
- Build solution **\*.sln
  - Type = Visual Studio Build
  - Version = 1.*
  - Solution = **\.sln
  - Visual Studio Version = Latest
  - Platform = $(BuildPlatform)
  - Configuration = $(BuildConfiguration)
- Test Assemblies
  - Type = Visual Studio Test
  - Version = 2.*
  - Select tests using = Test assemblies
  - Test assemblies = \**\$(BuildConfiguration)\*UnitTests.dll\n!**\obj\**
  - Search folder = $(System.DefaultWorkingDirectory)
  - Test filter criteria = TestCategory=UnitTests
  - Build Platform = $(BuildPlatform)
  - Build Configuration = $(BuildConfiguration)
- Validate ARM Templates: production
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure subscription = set appropriate
  - Action = Create or update resource group
  - Resource group = $(ValidateTemplatesResourceGroup)
  - Location = East US
  - Template location = Linked artifact
  - Template = infra/templates/deploy.json
  - Override template parameters = -functionAppName tmpforvalidation
  - Deployment mode = Validation only
- Validate ARM Templates: staging
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure subscription = set appropriate
  - Action = Create or update resource group
  - Resource group = $(ValidateTemplatesResourceGroup)
  - Location = East US
  - Template location = Linked artifact
  - Template = infra/templates/deploy-slot.json
  - Override template parameters = -functionAppName tmpforvalidation
  - Deployment mode = Validation only
- Remove temporary ValidateTemplatesResourceGroup
  - Type = Azure Resource Group Deployment
  - Version = 2.*
  - Azure subscription = set appropriate
  - Action = Delete resource group
  - Resource group = $(ValidateTemplatesResourceGroup)
- Copy Files: function
  - Type = Copy Files
  - Version = 2.*
  - Source Folder = $(build.sourcesdirectory)/src/DotNetFunction/bin/$(BuildConfiguration)/net461
  - Content = **
  - Target Folder = $(build.artifactstagingdirectory)
- Publish Artifact: function
  - Type = Publish Build Artifacts
  - Version = 1.*
  - Path to Publish = $(build.artifactstagingdirectory)
  - Artifact Name = function
  - Artifact Type = Server
- Publish Artifact: infra
  - Type = Publish Build Artifacts
  - Version = 1.*
  - Path to Publish = infra/templates
  - Artifact Name = infra
  - Artifact Type = Server
- Publish Artifact: scripts
  - Type = Publish Build Artifacts
  - Version = 1.*
  - Path to Publish = infra/scripts
  - Artifact Name = scripts
  - Artifact Type = Server