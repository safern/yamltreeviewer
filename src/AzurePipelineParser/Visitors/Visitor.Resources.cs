using AzurePipelineParser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.RepresentationModel;

namespace AzurePipelineParser.Visitors
{
    public partial class Visitor
    {
        public Resources VisitResources(YamlMappingNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var resources = new Resources();

            foreach (var child in node.Children)
            {
                switch (child.Key.ToString().ToLower())
                {
                    case "pipelines":
                        var pipelinesNode = child.Value as YamlSequenceNode;
                        if (pipelinesNode == null)
                            throw new ArgumentNullException(nameof(pipelinesNode));
                        foreach(var pipeline in pipelinesNode.Children)
                        {
                            var newPipelineResource = VisitPipelineResource(pipeline as YamlMappingNode);
                            resources.Pipelines.Add(newPipelineResource);
                            resources.Children.Add(newPipelineResource);
                        }
                        break;
                    case "builds":
                        // Do nothing for now. There is no official spec for build resource, so skip for now.
                        break;
                    case "repositories":
                        var repositoryNode = child.Value as YamlSequenceNode;
                        if (repositoryNode == null)
                            throw new ArgumentNullException(nameof(pipelinesNode));
                        foreach (var repository in repositoryNode.Children)
                        {
                            var newRepositoryResource = VisitRepositoryResource(repository as YamlMappingNode);
                            resources.Repositories.Add(newRepositoryResource);
                            resources.Children.Add(newRepositoryResource);
                        }
                        break;
                    case "containers":
                        var containersNode = child.Value as YamlSequenceNode;
                        if (containersNode == null)
                            throw new ArgumentNullException(nameof(pipelinesNode));
                        foreach (var container in containersNode.Children)
                        {
                            var newContainerResource = VisitContainerResource(container as YamlMappingNode);
                            resources.Containers.Add(newContainerResource);
                            resources.Children.Add(newContainerResource);
                        }
                        break;
                    case "packages":
                        var packagesNode = child.Value as YamlSequenceNode;
                        if (packagesNode == null)
                            throw new ArgumentNullException(nameof(packagesNode));
                        foreach (var package in packagesNode.Children)
                        {
                            var newPackageResource = VisitPackageResource(package as YamlMappingNode);
                            resources.Packages.Add(newPackageResource);
                            resources.Children.Add(newPackageResource);
                        }
                        break;
                    default:
                        throw new Exception("Unexpected resource type.");
                }
            }

            return resources;
        }

        public PackageResource VisitPackageResource(YamlMappingNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var packageResource = new PackageResource();

            foreach (var child in node.Children)
            {
                switch (child.Key.ToString().ToLower())
                {
                    case "package":
                        packageResource.Alias = child.Value.ToString();
                        break;
                    case "type":
                        packageResource.PackageType = (PackageType)Enum.Parse(typeof(PackageType), child.Value.ToString());
                        break;
                    case "connection":
                        packageResource.Connection = child.Value.ToString();
                        break;
                    case "name":
                        packageResource.Name = child.Value.ToString();
                        break;
                    case "version":
                        packageResource.Version = child.Value.ToString();
                        break;
                    case "trigger":
                        packageResource.Trigger = bool.Parse(child.Value.ToString());
                        break;
                    default:
                        throw new Exception("Unexpected property on package resource");
                }
            }

            return packageResource;
        }

        public ContainerResource VisitContainerResource(YamlMappingNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var containerResource = new ContainerResource();

            foreach (var child in node.Children)
            {
                switch (child.Key.ToString().ToLower())
                {
                    case "container":
                        containerResource.Identifier = child.Value.ToString();
                        break;
                    case "image":
                        containerResource.Image = child.Value.ToString();
                        break;
                    case "options":
                        containerResource.Options = child.Value.ToString();
                        break;
                    case "endpoint":
                        containerResource.Endpoint = child.Value.ToString();
                        break;
                    case "env":
                        var nodeAsMapping = child.Value as YamlMappingNode;
                        if (nodeAsMapping == null)
                            throw new Exception("Expected a sequence node for env");

                        foreach (var variable in nodeAsMapping.Children)
                        {
                            var newVar = VisitVariable(variable.Key.ToString(), variable.Value as YamlScalarNode);
                            containerResource.Env.Add(newVar);
                        }
                        break;
                    case "ports":
                        var nodeAsSequence = child.Value as YamlSequenceNode;
                        if (nodeAsSequence == null)
                            throw new Exception("Expected a sequence node for ports");

                        foreach(YamlScalarNode port in nodeAsSequence.Children)
                        {
                            var newPort = port.ToString();
                            containerResource.Ports.Add(newPort);
                        }
                        
                        break;
                    case "volumes":
                        var sequenceNode = child.Value as YamlSequenceNode;
                        if (sequenceNode == null)
                            throw new Exception("Expected a sequence node for ports");

                        foreach (YamlScalarNode volume in sequenceNode.Children)
                        {
                            var newPort = volume.ToString();
                            containerResource.Volumes.Add(newPort);
                        }

                        break;
                    case "mapdockersocket":
                        containerResource.MapDockerSocket = bool.Parse(child.Value.ToString());
                        break;
                    case "mountreadonly":
                        var mountReadOnly = VisitMountReadOnly(child.Value as YamlMappingNode);
                        containerResource.MountReadyOnly = mountReadOnly;
                        containerResource.Children.Add(mountReadOnly);
                        break;
                    default:
                        throw new Exception("Unexpected Pipeline Resource property");

                }
            }

            return containerResource;
        }

        public MountReadyOnly VisitMountReadOnly(YamlMappingNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var mountReadOnly = new MountReadyOnly();

            foreach(var child in node.Children)
            {
                switch (child.Key.ToString().ToLower())
                {
                    case "externals":
                        mountReadOnly.Externals = bool.Parse(child.Value.ToString());
                        break;
                    case "tasks":
                        mountReadOnly.Tasks = bool.Parse(child.Value.ToString());
                        break;
                    case "tools":
                        mountReadOnly.Tools = bool.Parse(child.Value.ToString());
                        break;
                    case "work":
                        mountReadOnly.Work = bool.Parse(child.Value.ToString());
                        break;
                    default:
                        throw new Exception("Unexpected mountReadOnly property");
                }
            }

            return mountReadOnly;
        }

        public RepositoryResource VisitRepositoryResource(YamlMappingNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var repositoryResource = new RepositoryResource();

            foreach (var child in node.Children)
            {
                switch (child.Key.ToString().ToLower())
                {
                    case "repository":
                        repositoryResource.Identifier = child.Value.ToString();
                        break;
                    case "type":
                        repositoryResource.RepositoryType = (RepositoryType)Enum.Parse(typeof(RepositoryType), child.Value.ToString());
                        break;
                    case "name":
                        repositoryResource.Name = child.Value.ToString();
                        break;
                    case "ref":
                        repositoryResource.Ref = child.Value.ToString();
                        break;
                    case "endpoint":
                        repositoryResource.Endpoint = child.Value.ToString();
                        break;
                    case "trigger":
                        var trigger = VisitPushTrigger(child.Value);
                        repositoryResource.Trigger = trigger;
                        repositoryResource.Children.Add(trigger);
                        break;
                    default:
                        throw new Exception("Unexpected Pipeline Resource property");

                }
            }

            return repositoryResource;
        }

        public PipelineResource VisitPipelineResource(YamlMappingNode? node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            var pipelineResource = new PipelineResource();

            foreach (var child in node.Children)
            {
                switch (child.Key.ToString().ToLower())
                {
                    case "pipeline":
                        pipelineResource.Identifier = child.Value.ToString();
                        break;
                    case "project":
                        pipelineResource.Project = child.Value.ToString();
                        break;
                    case "source":
                        pipelineResource.Source = child.Value.ToString();
                        break;
                    case "version":
                        pipelineResource.Version = child.Value.ToString();
                        break;
                    case "branch":
                        pipelineResource.Branch = child.Value.ToString();
                        break;
                    case "trigger":
                        var trigger = VisitPipelineTrigger(child.Value);
                        pipelineResource.Trigger = trigger;
                        pipelineResource.Children.Add(trigger);
                        break;
                    default:
                        throw new Exception("Unexpected Pipeline Resource property");

                }
            }

            return pipelineResource;
        }
    }
}
