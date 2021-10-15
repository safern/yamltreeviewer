using AzurePipelineParser.Models;
using AzurePipelineParser.Visitors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using YamlDotNet.RepresentationModel;

namespace AzurePipelineParser.Tests
{
    public class PipelineVisitorTests
    {
        public static IEnumerable<object[]> VariablesTestData()
        {
            yield return new object[] { @"variables:      # pipeline-level
  MY_VAR: 'my value'
  ANOTHER_VAR: 'another value'", @"{""VariableList"":[{""Name"":""MY_VAR"",""Value"":""my value"",""Group"":null,""IsReadOnly"":false,""Title"":""MY_VAR"",""Type"":6,""Children"":[]},{""Name"":""ANOTHER_VAR"",""Value"":""another value"",""Group"":null,""IsReadOnly"":false,""Title"":""ANOTHER_VAR"",""Type"":6,""Children"":[]}],""Title"":""variables"",""Type"":21,""Children"":[{""Title"":""MY_VAR"",""Type"":6,""Children"":[]},{""Title"":""ANOTHER_VAR"",""Type"":6,""Children"":[]}]}" };

            yield return new object[] { @"variables:
- name: MY_VARIABLE           # hard-coded value
  value: some value
  readonly: true
- group: my-variable-group-1  # variable group
- group: my-variable-group-2  # another variable group", @"{""VariableList"":[{""Name"":""MY_VARIABLE"",""Value"":""some value"",""Group"":null,""IsReadOnly"":true,""Title"":""MY_VARIABLE"",""Type"":6,""Children"":[]},{""Name"":null,""Value"":null,""Group"":""my-variable-group-1"",""IsReadOnly"":false,""Title"":null,""Type"":6,""Children"":[]},{""Name"":null,""Value"":null,""Group"":""my-variable-group-2"",""IsReadOnly"":false,""Title"":null,""Type"":6,""Children"":[]}],""Title"":""variables"",""Type"":21,""Children"":[{""Title"":""MY_VARIABLE"",""Type"":6,""Children"":[]},{""Title"":null,""Type"":6,""Children"":[]},{""Title"":null,""Type"":6,""Children"":[]}]}" };

            yield return new object[] { @"# File: component-x-pipeline.yml
variables:
- template: variables/build.yml  # Template reference", @"{""VariableList"":[],""Title"":""variables"",""Type"":21,""Children"":[{""Title"":null,""Type"":5,""Children"":[]}]}" };
        }

        [Theory]
        [MemberData(nameof(VariablesTestData))]
        public void CanParseVariables(string yaml, string expectedJson)
        {
            var reader = new StringReader(yaml);
            Visitor visitor = new Visitor(string.Empty, string.Empty);

            var yamlStream = new YamlStream();
            yamlStream.Load(reader);

            var root = yamlStream.Documents[0].RootNode as YamlMappingNode;
            var variableNode = root!.Children.Where(n => n.Key.ToString().Equals("variables")).FirstOrDefault();

            var result = visitor.VisitVariables(variableNode.Value);
            var serializedResult = JsonSerializer.Serialize(result);
            Assert.Equal(expectedJson, serializedResult);
        }

        public static IEnumerable<object[]> ResourcesTestData()
        {
            yield return new object[] { @"resources:
  pipelines:
  - pipeline: MyAppA
    source: MyCIPipelineA
  - pipeline: MyAppB
    source: MyCIPipelineB
    trigger: true
  - pipeline: MyAppC
    project:  DevOpsProject
    source: MyCIPipelineC
    branch: releases/M159
    version: 20190718.2
    trigger:
      branches:
        include:
        - main
        - releases/*
        exclude:
        - users/*", @"{""Pipelines"":[{""Identifier"":""MyAppA"",""Project"":null,""Source"":""MyCIPipelineA"",""Version"":null,""Branch"":null,""Tags"":[],""Trigger"":null,""Title"":""MyAppA"",""Type"":9,""Children"":[]},{""Identifier"":""MyAppB"",""Project"":null,""Source"":""MyCIPipelineB"",""Version"":null,""Branch"":null,""Tags"":[],""Trigger"":{""Branches"":{""Id"":""branches"",""Include"":[],""Exclude"":[],""Title"":""branches"",""Type"":15,""Children"":[]},""Tags"":[],""Stages"":[],""Title"":""trigger"",""Type"":14,""Children"":[]},""Title"":""MyAppB"",""Type"":9,""Children"":[{""Title"":""trigger"",""Type"":14,""Children"":[]}]},{""Identifier"":""MyAppC"",""Project"":""DevOpsProject"",""Source"":""MyCIPipelineC"",""Version"":""20190718.2"",""Branch"":""releases/M159"",""Tags"":[],""Trigger"":{""Branches"":{""Id"":""branches"",""Include"":[],""Exclude"":[],""Title"":""branches"",""Type"":15,""Children"":[]},""Tags"":[],""Stages"":[],""Title"":""trigger"",""Type"":14,""Children"":[]},""Title"":""MyAppC"",""Type"":9,""Children"":[{""Title"":""trigger"",""Type"":14,""Children"":[]}]}],""Repositories"":[],""Containers"":[],""Packages"":[],""Title"":""resources"",""Type"":8,""Children"":[{""Title"":""MyAppA"",""Type"":9,""Children"":[]},{""Title"":""MyAppB"",""Type"":9,""Children"":[{""Title"":""trigger"",""Type"":14,""Children"":[]}]},{""Title"":""MyAppC"",""Type"":9,""Children"":[{""Title"":""trigger"",""Type"":14,""Children"":[]}]}]}" };

            yield return new object[] { @"resources:
  repositories:
  - repository: common
    type: github
    name: Contoso/CommonTools
    endpoint: MyContosoServiceConnection", @"{""Pipelines"":[],""Repositories"":[{""Identifier"":""common"",""RepositoryType"":1,""Name"":""Contoso/CommonTools"",""Ref"":null,""Endpoint"":""MyContosoServiceConnection"",""Trigger"":null,""Title"":""common"",""Type"":12,""Children"":[]}],""Containers"":[],""Packages"":[],""Title"":""resources"",""Type"":8,""Children"":[{""Title"":""common"",""Type"":12,""Children"":[]}]}" };

            yield return new object[] { @"resources:
  containers:
  - container: linux
    image: ubuntu:16.04
    mapDockerSocket: false
  - container: windows
    image: myprivate.azurecr.io/windowsservercore:1803
    options: --privileged
    endpoint: my_acr_connection
    env:
      My_Var: hello world
      OtherVar: true
      NumerericVar: 89.0
  - container: my_service
    image: my_service:tag
    ports:
    - 8080:80 # bind container port 80 to 8080 on the host machine
    - 6379 # bind container port 6379 to a random available port on the host machine
    volumes:
    - /src/dir:/dst/dir # mount /src/dir on the host into /dst/dir in the container
    mountReadOnly:
      externals: true
      tasks: false
      tools: true
      work: False", @"{""Pipelines"":[],""Repositories"":[],""Containers"":[{""Identifier"":""linux"",""Image"":""ubuntu:16.04"",""Options"":null,""Endpoint"":null,""Env"":[],""Ports"":[],""Volumes"":[],""MapDockerSocket"":false,""MountReadyOnly"":null,""Title"":""linux"",""Type"":10,""Children"":[]},{""Identifier"":""windows"",""Image"":""myprivate.azurecr.io/windowsservercore:1803"",""Options"":""--privileged"",""Endpoint"":""my_acr_connection"",""Env"":[{""Name"":""My_Var"",""Value"":""hello world"",""Group"":null,""IsReadOnly"":false,""Title"":""My_Var"",""Type"":6,""Children"":[]},{""Name"":""OtherVar"",""Value"":""true"",""Group"":null,""IsReadOnly"":false,""Title"":""OtherVar"",""Type"":6,""Children"":[]},{""Name"":""NumerericVar"",""Value"":""89.0"",""Group"":null,""IsReadOnly"":false,""Title"":""NumerericVar"",""Type"":6,""Children"":[]}],""Ports"":[],""Volumes"":[],""MapDockerSocket"":true,""MountReadyOnly"":null,""Title"":""windows"",""Type"":10,""Children"":[]},{""Identifier"":""my_service"",""Image"":""my_service:tag"",""Options"":null,""Endpoint"":null,""Env"":[],""Ports"":[""8080:80"",""6379""],""Volumes"":[""/src/dir:/dst/dir""],""MapDockerSocket"":true,""MountReadyOnly"":{""Externals"":true,""Tasks"":false,""Tools"":true,""Work"":false,""Title"":""mountReadyOnly"",""Type"":11,""Children"":[]},""Title"":""my_service"",""Type"":10,""Children"":[{""Title"":""mountReadyOnly"",""Type"":11,""Children"":[]}]}],""Packages"":[],""Title"":""resources"",""Type"":8,""Children"":[{""Title"":""linux"",""Type"":10,""Children"":[]},{""Title"":""windows"",""Type"":10,""Children"":[]},{""Title"":""my_service"",""Type"":10,""Children"":[{""Title"":""mountReadyOnly"",""Type"":11,""Children"":[]}]}]}" };

            yield return new object[] { @"resources:
  packages:
    - package: contoso
      type: npm
      connection: pat-contoso
      name: yourname/contoso 
      version: 7.130.88 
      trigger: true", @"{""Pipelines"":[],""Repositories"":[],""Containers"":[],""Packages"":[{""Alias"":""contoso"",""PackageType"":1,""Connection"":""pat-contoso"",""Name"":""yourname/contoso"",""Version"":""7.130.88"",""Trigger"":true,""Title"":""contoso"",""Type"":13,""Children"":[]}],""Title"":""resources"",""Type"":8,""Children"":[{""Title"":""contoso"",""Type"":13,""Children"":[]}]}" };
        }

        [Theory]
        [MemberData(nameof(ResourcesTestData))]
        public void CanParseResources(string yaml, string expectedJson)
        {
            var reader = new StringReader(yaml);
            Visitor visitor = new Visitor(string.Empty, string.Empty);

            var yamlStream = new YamlStream();
            yamlStream.Load(reader);

            var root = yamlStream.Documents[0].RootNode as YamlMappingNode;
            var variableNode = root!.Children.Where(n => n.Key.ToString().Equals("resources")).FirstOrDefault();

            var result = visitor.VisitResources(variableNode.Value as YamlMappingNode);
            var serializedResult = JsonSerializer.Serialize(result);
            Assert.Equal(expectedJson, serializedResult);
        }

        public static IEnumerable<object[]> TriggersTestData()
        {
            yield return new object[] { @"trigger:
- main
- develop", @"{""Name"":null,""BranchNames"":[""main"",""develop""],""Batch"":false,""Disabled"":false,""Branches"":{""Id"":""branches"",""Include"":[],""Exclude"":[],""Title"":""branches"",""Type"":15,""Children"":[]},""Tags"":{""Id"":""tags"",""Include"":[],""Exclude"":[],""Title"":""tags"",""Type"":15,""Children"":[]},""Paths"":{""Id"":""paths"",""Include"":[],""Exclude"":[],""Title"":""paths"",""Type"":15,""Children"":[]},""Title"":""trigger"",""Type"":14,""Children"":[]}" };

            yield return new object[] { @"trigger: none # will disable CI builds (but not PR builds)", @"{""Name"":null,""BranchNames"":[],""Batch"":false,""Disabled"":true,""Branches"":{""Id"":""branches"",""Include"":[],""Exclude"":[],""Title"":""branches"",""Type"":15,""Children"":[]},""Tags"":{""Id"":""tags"",""Include"":[],""Exclude"":[],""Title"":""tags"",""Type"":15,""Children"":[]},""Paths"":{""Id"":""paths"",""Include"":[],""Exclude"":[],""Title"":""paths"",""Type"":15,""Children"":[]},""Title"":""trigger"",""Type"":14,""Children"":[]}" };
            
            yield return new object[] { @"trigger:
  batch: true
  branches:
    include:
    - features/*
    exclude:
    - features/experimental/*
  paths:
    exclude:
    - README.md", @"{""Name"":null,""BranchNames"":[],""Batch"":true,""Disabled"":false,""Branches"":{""Id"":null,""Include"":[""[ features/* ]""],""Exclude"":[""[ features/experimental/* ]""],""Title"":null,""Type"":15,""Children"":[]},""Tags"":{""Id"":""tags"",""Include"":[],""Exclude"":[],""Title"":""tags"",""Type"":15,""Children"":[]},""Paths"":{""Id"":null,""Include"":[],""Exclude"":[""[ README.md ]""],""Title"":null,""Type"":15,""Children"":[]},""Title"":""trigger"",""Type"":14,""Children"":[{""Title"":null,""Type"":15,""Children"":[]},{""Title"":null,""Type"":15,""Children"":[]}]}" };
        }

        [Theory]
        [MemberData(nameof(TriggersTestData))]
        public void CanParseTriggers(string yaml, string expectedJson)
        {
            var reader = new StringReader(yaml);
            Visitor visitor = new Visitor(string.Empty, string.Empty);

            var yamlStream = new YamlStream();
            yamlStream.Load(reader);

            var root = yamlStream.Documents[0].RootNode as YamlMappingNode;
            var triggerNode = root!.Children.Where(n => n.Key.ToString().Equals("trigger")).FirstOrDefault();

            var result = visitor.VisitPushTrigger(triggerNode.Value);
            var serializedResult = JsonSerializer.Serialize(result);
            Assert.Equal(expectedJson, serializedResult);
        }
    }
}
