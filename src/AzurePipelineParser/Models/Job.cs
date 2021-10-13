using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Job : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public string? DisplayName { get; set; }

        public IList<string> DependsOn { get; set; } = new List<string>();

        public string? Condition { get; set; }

        // public Strategy? Strategy { get; set; } ToDo: this is a more complex type, should do it later

        public bool ContinueOnError { get; set; }

        public Pool? Pool { get; set; }

        public Workspace? Workspace { get; set; }

        public ContainerReference? Container { get; set; }

        public int? TimeoutInMinutes { get; set; }

        public int? CancelTimeoutInMinutes { get; set; }

        public Variables? Variables { get; set; }

        public IList<Step> Steps { get; set; } = new List<Step>();

        public Uses? Uses { get; set; }

        public string? Title => DisplayName;

        public PipelineNodeTypes Type => PipelineNodeTypes.Job;

        public IList<IPipelineNode> Children => _children;
    }
}
