using System;
using System.Collections.Generic;
using System.Text;

namespace AzurePipelineParser.Models
{
    public interface IPipelineNode
    {
        string? Title { get; }

        PipelineNodeTypes Type { get; }

        IList<IPipelineNode> Children { get; }
    }
}
