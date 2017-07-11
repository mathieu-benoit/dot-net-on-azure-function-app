Here is one example to Build an .NET method to an Azure Function App via VSTS. You could adapt it with your own context, needs and constraints.

![Build Overview](/docs/imgs/DotNet-FunctionApp-CI.PNG)

# Import the Build Definition

You could import [the associated Build Definition stored in this repository](/vsts/DotNet-FunctionApp-CI.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create manually the Build Definition

## Variables
- BuildConfiguration = release

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
  - Test assemblies = \**\$(BuildConfiguration)\*test*.dll\n!**\obj\**
  - Search folder = $(System.DefaultWorkingDirectory)
  - Build Platform = $(BuildPlatform)
  - Build Configuration = $(BuildConfiguration)
- Copy Files
  - Type = Copy Files
  - Version = 2.*
  - Source Folder = $(build.sourcesdirectory)/src/DotNetFunction/bin/$(BuildConfiguration)/net461
  - Content = **
  - Target Folder = $(build.artifactstagingdirectory)
- Publish Artifact
  - Type = Publish Build Artifacts
  - Version = 1.*
  - Path to Publish = $(build.artifactstagingdirectory)
  - Artifact Name = drop
  - Artifact Type = Server