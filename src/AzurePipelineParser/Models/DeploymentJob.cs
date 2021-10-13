using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public class DeploymentJob : Job
    {
        public string? Deployment { get; set; }

        public IList<string> Services { get; set; } = new List<string>();

        public DeploymentEnvironment? Environment { get; set; }
    }
}
