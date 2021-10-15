using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Resources : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public PipelineResources? Pipelines { get; set; }

        // public BuildResource? Builds { get; set; } ToDo: Find schema for this type of resource

        public RepositoryResources? Repositories { get; set; }

        public ContainerResources? Containers { get; set; }

        public PackageResources? Packages { get; set; }

        public string? Title => "resources";

        public PipelineNodeTypes Type => PipelineNodeTypes.Resources;

        public IList<IPipelineNode> Children => _children;
    }
}
