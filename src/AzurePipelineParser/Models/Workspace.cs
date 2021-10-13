using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class Workspace : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Clean { get; set; }

        public string? Title => $"workspace: clean = {Clean}";

        public PipelineNodeTypes Type => PipelineNodeTypes.Workspace;

        public IList<IPipelineNode> Children => _children;
    }
}
