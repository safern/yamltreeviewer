using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class PackageResources : ResourcesCollection
    {
        public IList<PackageResource> Packages { get; set; } = new List<PackageResource>();

        public override string? Title => "package resources";
    }
}
