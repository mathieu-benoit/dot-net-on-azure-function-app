Here is one example to Build a .NET method to be deployed as an Azure Function App via VSTS. You could adapt it with your own context, needs and constraints.

2 ways to create the associated Build Definition:

- [Create a YAML Build Definition](#create-a-yaml-build-definition)
- [Create a manual Build Definition](#create-a-manual-build-definition)
  - [Variables](#variables)
  - [Repository](#repository)
  - [Triggers](#triggers)
  - [Options](#options)
  - [Process - Build process](#process---build-process)
  - [Tasks](#tasks)

![Build Overview](/docs/imgs/DotNet-FunctionApp-CI.PNG)

# Create a YAML Build Definition

You could import [the associated Build Definition stored in this repository](/vsts/DotNet-FunctionApp-CI.json) and then follow these steps to adapt it to your current project, credentials, etc.:

TODO

# Create a manual Build Definition

## Variables

- BuildConfiguration = release
- BuildPlatform = any cpu

## Repository

- Repository Type = GitHub
- Connection = set appropriate
- Repository = mathieu-benoit/dot-net-on-azure-function-app
- Default branch = master

## Triggers

- Continuous Integration = Enabled
  - Choose the correct repository
  - Branch Filters
    - Type = Include ; Branch specification = master

## Options

- Create work item on failure = Enabled
  - Type = Bug
  - Assign to requestor = true

## Process - Build process

- Name = DotNet-FunctionApp-CI
- Default agent queue = Hosted VS2017

## Tasks 

Add manually the tasks and associated values according [this YAML definition file](../vsts/DotNet-FunctionApp-CI.yml).