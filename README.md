Sample .NET method deployed on an Azure Function App.

# History of changes
- July 2017 - Initial setup.

# Build Definition with VSTS

- [Build - CI](/docs/DotNet-FunctionApp-CI.md)

# Release Definition with VSTS

- [Release - CD](/docs/DotNet-FunctionApp-CD.md)

# Alternatives

- Instead of having a compiled .NET method/library, use static files with C#, NodeJS, etc.
- Instead of using a Consumption Plan to host the Azure Function, use an App Service Plan to leverage its capabilities: more slots, etc.