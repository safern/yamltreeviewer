using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Pipeline : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public Resources? Resources { get; set; }

        public IList<Variable> Variables { get; set; } = new List<Variable>();

        public PushTrigger? Trigger { get; set; }

        public PullRequestTrigger? Pr { get; set; }

        public IList<Stage> Stages { get; set; } = new List<Stage>();

        public IList<Job> Jobs { get; set; } = new List<Job>();

        public IList<Step> Steps { get; set; } = new List<Step>();

        public string? Title => Name;

        public PipelineNodeTypes Type => PipelineNodeTypes.Pipeline;

        public IList<IPipelineNode> Children => _children;
    }
}
