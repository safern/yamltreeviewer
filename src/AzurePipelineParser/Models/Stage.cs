using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Stage : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public string? DisplayName { get; set; }

        public string? Title => DisplayName;

        public IList<string> DependsOn { get; set; } = new List<string>();

        public string? Condition { get; set; }

        public IList<Variable> Variables { get; set; } = new List<Variable>();

        public IList<Job> Jobs { get; set; } = new List<Job>();

        public PipelineNodeTypes Type => PipelineNodeTypes.Stage;

        public IList<IPipelineNode> Children => _children;
    }
}
