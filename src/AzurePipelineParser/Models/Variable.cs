using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Variable : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public string? Value { get; set; }

        public string? Group { get; set; }

        public bool IsReadOnly { get; set; }

        public string? Title => Name;

        public PipelineNodeTypes Type => PipelineNodeTypes.Variable;

        public IList<IPipelineNode> Children => _children;
    }
}
