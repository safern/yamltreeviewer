using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class PackageResource : IPipelineNode
    {
        private List<IPipelineNode> _children = new List<IPipelineNode>();

        public string? Alias { get; set; }

        public PackageType PackageType { get; set; }

        public string? Connection { get; set; }

        public string? Name { get; set; }

        public string? Version { get; set; }

        public bool? Trigger { get; set; }

        public string? Title => Alias;

        public PipelineNodeTypes Type => PipelineNodeTypes.PackageResource;

        public IList<IPipelineNode> Children => _children;
    }

    public enum PackageType
    {
        NuGet,
        Npm
    }
}
