using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class PipelineResource : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Identifier { get; set; }

        public string? Project { get; set; }

        public string? Source { get; set; }

        public string? Version { get; set; }

        public string? Branch { get; set; }

        public IList<string> Tags { get; set; } = new List<string>();

        public PipelineTrigger? Trigger { get; set; }

        public string? Title => Identifier;

        public PipelineNodeTypes Type => PipelineNodeTypes.PipelineResource;

        public IList<IPipelineNode> Children => _children;
    }
}
