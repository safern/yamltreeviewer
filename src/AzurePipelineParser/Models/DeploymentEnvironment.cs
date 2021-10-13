using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class DeploymentEnvironment : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public string? ResourceName { get; set; }

        public string? ResourceId { get; set; }

        public ResourceType? ResourceType {  get; set; }

        public IList<string> Tags { get; set; } = new List<string>();

        public string? Title => Name;

        public PipelineNodeTypes Type => PipelineNodeTypes.Environment;

        public IList<IPipelineNode> Children => _children;
    }

    public enum ResourceType
    {
        VirtualMachine,
        Kubernetes
    }
}
