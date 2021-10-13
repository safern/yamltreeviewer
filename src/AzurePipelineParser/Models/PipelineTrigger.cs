using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class PipelineTrigger : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public IncludeExclude? Branches { get; set; } = new IncludeExclude() { Id = "branches" };

        public IList<string> Tags { get; set; } = new List<string>();

        public IList<string> Stages { get; set; } = new List<string>();

        public string? Title => "trigger";

        public PipelineNodeTypes Type => PipelineNodeTypes.Trigger;

        public IList<IPipelineNode> Children => _children;
    }
}
