using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public abstract class Step : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? DisplayName { get; set; }

        public string? Name { get; set; }

        public string? Condition { get; set; }

        public bool ContinueOnError { get; set; }

        public bool Enabled { get; set; }

        public Variables? Env { get; set; }

        public int? timeoutInMinutes { get; set; }

        public string? Title => DisplayName;

        public IList<IPipelineNode> Children => _children;

        public PipelineNodeTypes Type => throw new NotImplementedException(); // Implemented on concrete types.
    }
}
