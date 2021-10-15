using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class PipelineResources : ResourcesCollection
    {
        public IList<PipelineResource> Pipelines { get; set; } = new List<PipelineResource>();

        public override string? Title => "pipeline resources";
    }
}
