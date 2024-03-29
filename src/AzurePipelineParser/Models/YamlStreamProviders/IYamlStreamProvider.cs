﻿using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AzurePipelineParser.Models.YamlStreamProviders
{
    public interface IYamlStreamProvider
    {
        Task<Stream> GetStreamFromPathAsync(string path, CancellationToken cancellationToken = default); 
    }
}
