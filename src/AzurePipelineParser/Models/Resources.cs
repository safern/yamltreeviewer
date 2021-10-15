using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Resources : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public IList<PipelineResource> Pipelines { get; set; } = new List<PipelineResource>();

        // public BuildResource? Builds { get; set; } ToDo: Find schema for this type of resource

        public IList<RepositoryResource> Repositories { get; set; } = new List<RepositoryResource>();

        public IList<ContainerResource> Containers { get; set; } = new List<ContainerResource>();

        public IList<PackageResource> Packages { get; set; } = new List<PackageResource>();

        public string? Title => "resources";

        public PipelineNodeTypes Type => PipelineNodeTypes.Resources;

        public IList<IPipelineNode> Children => _children;
    }
}
