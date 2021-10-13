using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class PullRequestTrigger : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public IList<string> BranchNames { get; set; } = new List<string>();

        public bool AutoCancel { get; set; }

        public IncludeExclude? Branches { get; set; } = new IncludeExclude() { Id = "branches" };

        public IncludeExclude? Paths { get; set; } = new IncludeExclude() { Id = "paths" };

        public bool? Drafts { get; set; }

        public string? Title => "pr";

        public PipelineNodeTypes Type => PipelineNodeTypes.Trigger;

        public IList<IPipelineNode> Children => _children;
    }
}
