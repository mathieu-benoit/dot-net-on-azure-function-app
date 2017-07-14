ALM and DevOps practices with a sample .NET method hosted in Azure Function App.

# History of changes

- July 2017 - Initial setup.

# Overview

![Process - Overview](/docs/imgs/Process-Overview.PNG)

# Build Definition with VSTS

Details could be found here: [Build - CI](/docs/DotNet-FunctionApp-CI.md)

Here are the DevOps practices highlighted within this CI pipeline:
- CI/Build triggered at each commit
- Build the .NET library
- Unit tests the .NET method
- Infrastructure as Code with the ARM Templates and the PowerShell scripts
- ARM Templates validation
- Expose artifacts to be used then by the CD pipeline (.NET library, ARM Templates and PowerShell scripts)
- Create a bug work item on build failure (assign to requestor)

# Release Definition with VSTS

Details could be found here: [Release - CD](/docs/DotNet-FunctionApp-CD.md)

Here are the DevOps practices highlighted within this CD pipeline:
- CD triggered at each CI/Build succesfully completed
- Infrastructure as Code with the ARM Templates and the PowerShell scripts
- Deploy the .NET method on the Azure serverless service: Azure Function App
- Use the Staging Slot mechanism with the associated Swap action to minimize downtime while upgrading the Production
- Securing the production environment by adding a Lock on the associated Azure Resource Group
- Monitor the Function App by using Application Insights

# Other Misc DevOps practices implemented

- CI/CD definitions as Code with the exported json file of the Build and Release Definitions

# Alternatives and potantial further considerations

- Describre local setup
- Add Integration Tests within the CD pipeline
- Instead of having a compiled .NET method/library, use static files with C#, NodeJS, etc.
- Instead of using a Consumption Plan to host the Azure Function, use an App Service Plan to leverage its capabilities: more slots, etc.
- Instead of allowing 'anonymous' request you could/should setup another AuthorizationLevel and then [retrieve by code the key](https://stackoverflow.com/questions/43253453/get-function-host-keys-of-azure-function-in-powershell/44117841#44117841) for your URL ping test during your CD pipeline.
