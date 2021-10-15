using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class RepositoryResources : ResourcesCollection
    {
        public IList<RepositoryResource> Repositories { get; set; } = new List<RepositoryResource>();

        public override string? Title => "repository resources";
    }
}
