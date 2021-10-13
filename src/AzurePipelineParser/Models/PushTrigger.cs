using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class PushTrigger : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public IList<string> BranchNames { get; set; } = new List<string>();

        public bool Batch { get; set; }

        public IncludeExclude? Branches { get; set; } = new IncludeExclude() { Id = "branches" };

        public IncludeExclude? Tags { get; set; } = new IncludeExclude() { Id = "tags" };

        public IncludeExclude? Paths { get; set; } = new IncludeExclude() { Id = "paths" };

        public string? Title => "trigger";

        public PipelineNodeTypes Type => PipelineNodeTypes.Trigger;

        public IList<IPipelineNode> Children => _children;
    }

    public class IncludeExclude : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Id { get; set; }

        public IList<string> Include { get; set; } = new List<string>();

        public IList<string> Exclude { get; set; } = new List<string>();

        public string? Title => Id;

        public PipelineNodeTypes Type => PipelineNodeTypes.IncludeExclude;

        public IList<IPipelineNode> Children => _children;
    }
}
