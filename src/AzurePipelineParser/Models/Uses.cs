using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Uses : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public IList<string> Repositories { get; set; } = new List<string>();

        public IList<string> Pools { get; set; } = new List<string>();

        public string? Title => "uses";

        public PipelineNodeTypes Type => PipelineNodeTypes.Uses;

        public IList<IPipelineNode> Children => _children;
    }
}
