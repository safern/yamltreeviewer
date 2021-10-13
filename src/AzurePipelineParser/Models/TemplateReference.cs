using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class TemplateReference : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Name { get; set; }

        public IList<Parameter> Parameters { get; set; } = new List<Parameter>();

        public string? Title => Name;

        public PipelineNodeTypes Type => PipelineNodeTypes.Template;

        public IList<IPipelineNode> Children => _children;
    }
}
