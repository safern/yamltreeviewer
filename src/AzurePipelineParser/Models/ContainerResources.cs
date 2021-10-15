using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class ContainerResources : ResourcesCollection
    {
        public IList<ContainerResource> Containers { get; set; } = new List<ContainerResource>();

        public override string? Title => "container resources";
    }
}
