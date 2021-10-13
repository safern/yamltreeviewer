using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Resources : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public PipelineResource? Pipelines { get; set; } 

        // public BuildResource? Builds { get; set; } ToDo: Find schema for this type of resource

        public RepositoryResource? Repositories { get; set; }

        public ContainerResource? Containers { get; set; }

        public PackageResource? Packages { get; set; }

        public string? Title => "resources";

        public PipelineNodeTypes Type => PipelineNodeTypes.Resources;

        public IList<IPipelineNode> Children => _children;
    }
}
