using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace AzurePipelineParser.Models.YamlStreamProviders
{
    public class ZipStreamProvider : IYamlStreamProvider, IAsyncDisposable
    {
        private Stream? _zipStream;
        private Lazy<Dictionary<string, Stream>> _zipStreamData;
        private ZipArchive _zipArchive;

        public ZipStreamProvider(Stream zipStream)
        {
            _zipStream = zipStream;
            _zipArchive = new ZipArchive(_zipStream, ZipArchiveMode.Read, leaveOpen: true);
            _zipStreamData = new Lazy<Dictionary<string, Stream>>(InitZipStreamData);
        }

        public Stream GetStreamFromPath(string path)
        {
            if (path.StartsWith('/'))
            {
                path = path.TrimStart('/');
            }

            if (!_zipStreamData.Value.TryGetValue(path, out Stream result))
            {
                throw new FileNotFoundException(path);
            }

            if (result.CanSeek && result.Position > 0)
            {
                result.Seek(0, SeekOrigin.Begin);
            }

            return result;
        }

        public async ValueTask DisposeAsync()
        {
            if (_zipArchive != null)
            {
                _zipArchive.Dispose();
                _zipArchive = null!;
            }

            if (_zipStream != null)
            {
                _zipStream.Dispose();
                _zipStream = null;
            }

            if (_zipStreamData != null && _zipStreamData.IsValueCreated)
            {
                foreach (Stream stream in _zipStreamData.Value.Values)
                    await stream.DisposeAsync();

                _zipStreamData.Value.Clear();
                _zipStreamData = null!;
            }
        }

        private Dictionary<string, Stream> InitZipStreamData()
        {
            Dictionary<string, Stream> data = new(_zipArchive.Entries.Count);
            foreach (ZipArchiveEntry entry in _zipArchive.Entries)
            {
                if (entry.Name.EndsWith(".yml", StringComparison.OrdinalIgnoreCase))
                {
                    data.Add(entry.FullName, entry.Open());
                }
            }

            return data;
        }
    }
}
