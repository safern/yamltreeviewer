using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public abstract class ResourcesCollection : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public abstract string? Title { get; }

        public PipelineNodeTypes Type => PipelineNodeTypes.Resources;

        public IList<IPipelineNode> Children => _children;
    }

    public enum ResourcesCollectionType
    {
        Pipelines,
        Repositories,
        Containers,
        Packages
    }
}
