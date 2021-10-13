using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class RepositoryResource : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Identifier { get; set; }

        public RepositoryType? RepositoryType { get; set; }

        public string? Name { get; set; }

        public string? Ref { get; set; }

        public string? Endpoint { get; set; }

        public PushTrigger? Trigger { get; set; }

        public string? Title => Identifier;

        public PipelineNodeTypes Type => PipelineNodeTypes.RepositoryResource;

        public IList<IPipelineNode> Children => _children;
    }

    public enum RepositoryType
    {
        git,
        github,
        bitbucket
    }
}
