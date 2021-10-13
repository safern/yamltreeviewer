using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class ContainerResource : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Identifier { get; set; }

        public string? Image { get; set; }

        public string? Options { get; set; }

        public string? Endpoint { get; set; }

        public Variables? Env { get; set; }
        
        public IList<string> Ports { get; set; } = new List<string>();

        public IList<string> Volumes { get; set; } = new List<string>();

        public bool MapDockerSocket { get; set; } = true;

        public MountReadyOnly? MountReadyOnly { get; set; }

        public string? Title => Identifier;

        public PipelineNodeTypes Type => PipelineNodeTypes.ContainerResource;

        public IList<IPipelineNode> Children => _children;
    }

    public class MountReadyOnly : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public bool? Externals { get; set; }

        public bool? Tasks { get; set; }

        public bool? Tools { get; set; }

        public bool? Work { get; set; }

        public string? Title => "mountReadyOnly";

        public PipelineNodeTypes Type => PipelineNodeTypes.MountReadyOnly;

        public IList<IPipelineNode> Children => _children;
    }
}
