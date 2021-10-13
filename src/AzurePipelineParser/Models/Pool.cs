using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Pool : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public IList<string> Demands { get; set; } = new List<string>();

        public string? VmImage { get; set; }

        public string? Title => Name;

        public PipelineNodeTypes Type => PipelineNodeTypes.Pool;

        public IList<IPipelineNode> Children => _children;
    }
}
