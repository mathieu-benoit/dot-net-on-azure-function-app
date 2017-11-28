Here is one example to Build a .NET method to be deployed as an Azure Function App via VSTS. You could adapt it with your own context, needs and constraints.

![Build Overview](/docs/imgs/DotNet-FunctionApp-CI.PNG)

Let's [create a new YAML build definition](https://docs.microsoft.com/en-us/vsts/build-release/actions/build-yaml#manually-create-a-yaml-build-definition).

*For now, the graphical representation of the tasks doesn't exist with the YAML definition. If you would like you could manually reproduce the tasks defined [in this file](../vsts/DotNet-FunctionApp-CI.yml) via the UI editor.*

- **Repository**
  - Repository Type = GitHub
  - Connection = set appropriate
  - Repository = `mathieu-benoit/dot-net-on-azure-function-app`
  - Default branch = `master`
- **Process - Build process**
  - Name = `DotNetMethodOnFunctionApp-CI`
  - Default agent queue = `Hosted VS2017`
  - YAML path = [`vsts/DotNet-FunctionApp-CI.yml`](../vsts/DotNet-FunctionApp-CI.yml)
- **Triggers**
  - Continuous Integration = Enabled
    - Choose the correct repository
    - Branch Filters
      - Type = Include ; Branch specification = master
- **Options** - *For now, not available with the YAML build definition.*
  - Create work item on failure = Enabled
    - Type = Bug
    - Assign to requestor = true