using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class ContainerReference : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? ImageName { get; set; }

        public string? Options { get; set; }

        public string? Endpoint { get; set; }

        public IList<Variable> Env { get; set; } = new List<Variable>();

        public string? Title => ImageName;

        public PipelineNodeTypes Type => PipelineNodeTypes.ContainerReference;

        public IList<IPipelineNode> Children => _children;
    }
}
