using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Parameter : IPipelineNode
    {
        private IList<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public IList<string> Values { get; set; } = new List<string>();

        public IList<string> Default { get; set; } = new List<string>();

        public ParameterTypes ParameterType { get; set; }

        public string? Title => Name;

        public PipelineNodeTypes Type => PipelineNodeTypes.Parameter;

        public IList<IPipelineNode> Children => _children;
    }

    public enum ParameterTypes
    {
        @string,
        number,
        boolean,
        @object,
        step,
        stepList,
        job,
        jobList,
        deployment,
        deploymentList,
        stage,
        stageList
    }
}
