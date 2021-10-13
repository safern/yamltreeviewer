using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Variables : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public List<Variable> VariableList { get; set; } = new List<Variable>();

        public string? Title => "variables";

        public PipelineNodeTypes Type => PipelineNodeTypes.VariableList;

        public IList<IPipelineNode> Children => _children;
    }
}
