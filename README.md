ALM and DevOps practices with a sample .NET method hosted in Azure Function App.

TOC

- [History of changes](#history-of-changes)
- [Overview](#overview)
- [Build Definition with VSTS](#build-definition-with-vsts)
- [Release Definition with VSTS](#release-definition-with-vsts)
- [Other Misc DevOps practices implemented](#other-misc-devops-practices-implemented)
- [Alternatives and potantial further considerations](#alternatives-and-potantial-further-considerations)
- [Resources](#resources)

# History of changes

- July 2017 - Initial setup.

# Overview

![Process - Overview](/docs/imgs/Process-Overview.PNG)

The goal of this GitHub repository is to demonstrate and use DevOps practices by leveraging a very simple compiled .NET method on Azure Function Apps (this Function is just an HttpTrigger saying "Hello" to the name passed in the querystring parameter).

Locally you will need to configure [Visual Studio 2017 Preview version 15.3 with the Azure Functions Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio).

By opening the .sln with Visual Studio you should see the structure of the solution like this:

![Visual Studio Solution Structure Overview](/docs/imgs/Visual-Studio-Solution-Structure-Overview.PNG)

To be able to setup the Build and Release definitions within VSTS described in the section below, you will need a Team Services (VSTS) account. If you don't have one, you could create it for free [here](https://www.visualstudio.com/team-services/).

To be able to deploy  the Azure services (Function App, Application Insights, etc.), you will need an Azure subscription. If you don't have one, you could create it for free [here](https://azure.microsoft.com/fr-ca/free/).

# Build Definition with VSTS

Details could be found here: [Build - CI](/docs/DotNet-FunctionApp-CI.md)

Here are the DevOps practices highlighted within this CI pipeline:
- CI/Build triggered at each commit on the master branch
- Compile the .NET library
- Run Unit tests of the .NET method
- Infrastructure as Code with the ARM Templates and the PowerShell scripts
- Run ARM Templates validation
- Expose artifacts to be used then by the CD pipeline (.NET library, ARM Templates, PowerShell scripts and Integration tests dlls)
- Create a bug work item on build failure (assign to requestor)

# Release Definition with VSTS

Details could be found here: [Release - CD](/docs/DotNet-FunctionApp-CD.md)

Here are the DevOps practices highlighted within this CD pipeline:
- CD triggered at each CI/Build succesfully completed
- Deploy the Infrastructure as Code with the ARM Templates and the PowerShell scripts
- Deploy the .NET method on the Azure serverless service: Azure Function App
- Run IntegrationTests once the Function App is deployed on Staging
- Use the Staging Slot mechanism with the associated Swap action to minimize downtime while upgrading to Production
- Securing the production environment by adding a Lock on the associated Azure Resource Group
- Monitor the Function App by using Application Insights
- Have a dedicated VSTS Release Environment defined for Rollback automated actions.

# Other Misc DevOps practices implemented

- GitHub as source control to leverage key features for collaboration such as feature-branch with pull request, etc.
- CI/CD definitions as Code with the exported json file of the Build and Release Definitions

# Alternatives and potential further considerations

- Improvements
    - Instead of allowing 'anonymous' request you could/should setup another AuthorizationLevel, for security reason, and then [retrieve by ARM Template the key](https://stackoverflow.com/questions/43253453/get-function-host-keys-of-azure-function-in-powershell/44117841#44117841) or [by REST API](https://github.com/Azure/azure-webjobs-sdk-script/wiki/Key-management-API) for your URL ping test during your CD pipeline. I started that by trying to call later on the pipeline the [get-function-outputs.json template](/infra/templates/get-function-outputs.json) but it's not returning the correct Function Key. Still need to investigate around that...
    - [Configure VSTS and Microsoft Teams](https://almvm.azurewebsites.net/labs/vsts/teams/) (or Slack or HipChat, etc.) to add more collaboration by setting up notifications once a work item is updated, a commit is done, a build or release or done, etc.
    - Instead of just having a Production environment with its staging slot, having a QA environment with its associated staging too.
- Alternatives
    - Instead of having a compiled .NET method/library, use static files with C#, NodeJS, etc.
    - Instead of hosting the Azure Function App in a [Consumption Plan, host it in an App Service Plan](https://docs.microsoft.com/en-us/azure/azure-functions/functions-scale).

# Resources

- [A day in the life of an Azure serverless developer](https://channel9.msdn.com/Events/Build/2017/T6003)
- [Create your first function using Visual Studio](https://docs.microsoft.com/en-us/azure/azure-functions/functions-create-your-first-function-visual-studio)
- [Automate resource deployment for your function app in Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-infrastructure-as-code)
- [Serverless computing with Azure Function in a DevOps Model](https://blogs.msdn.microsoft.com/troubleshooting_tips_for_developers/2017/07/05/serverless-computing-with-azure-function-in-a-devops-model/)
- [Deployment Slots Preview for Azure Functions](https://blogs.msdn.microsoft.com/appserviceteam/2017/06/13/deployment-slots-preview-for-azure-functions/)
- [Application Insights integration with Functions now in preview](https://blogs.msdn.microsoft.com/appserviceteam/2017/05/10/application-insights-integration-with-functions-now-in-preview/)
- Deploying Azure Function App with Deployment Slots using ARM Templates - Blog series: [First](https://nascent.blog/2017/05/31/azure-function-app-deployment-slots-arm-template/), [Second](https://nascent.blog/2017/06/22/azure-functions-arm-templates-snags-1-http-triggers-keys/) and [Third](https://nascent.blog/2017/06/27/azure-functions-slots-arm-templates-snags-2-redeploy-causes-unwanted-swap/)
- [Is Your Serverless Application Testable? – Azure Functions](https://blog.kloud.com.au/2017/07/22/is-your-serverless-application-testable-azure-functions/)
