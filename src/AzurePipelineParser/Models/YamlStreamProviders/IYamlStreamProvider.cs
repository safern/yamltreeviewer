
using System.IO;

namespace AzurePipelineParser.Models.YamlStreamProviders
{
    public interface IYamlStreamProvider
    {
        Stream GetStreamFromPath(string path); 
    }
}
