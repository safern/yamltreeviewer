using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace AzurePipelineParser.Models.YamlStreamProviders
{
    public class ZipStreamProvider : IYamlStreamProvider, IAsyncDisposable
    {
        private Stream? _zipStream;
        private Lazy<ConcurrentDictionary<string, Stream>> _zipStreamData;
        private ZipArchive _zipArchive;

        public ZipStreamProvider(Stream zipStream)
        {
            _zipStream = zipStream;
            _zipArchive = new ZipArchive(_zipStream, ZipArchiveMode.Read, leaveOpen: true);
            _zipStreamData = new Lazy<ConcurrentDictionary<string, Stream>>(InitZipStreamData);
        }

        public async Task<Stream> GetStreamFromPath(string path, CancellationToken cancellationToken = default)
        {
            ConcurrentDictionary<string, Stream> dictionary = _zipStreamData.Value;

            return await Task.Run(() =>
            {
                if (path.StartsWith('/'))
                {
                    path = path.TrimStart('/');
                }

                if (!dictionary.TryGetValue(path, out Stream result))
                {
                    throw new FileNotFoundException(path);
                }

                if (result.CanSeek && result.Position > 0)
                {
                    result.Seek(0, SeekOrigin.Begin);
                }

                return result;
            }, cancellationToken);
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

        private ConcurrentDictionary<string, Stream> InitZipStreamData()
        {
            ConcurrentDictionary<string, Stream> data = new();
            foreach (ZipArchiveEntry entry in _zipArchive.Entries)
            {
                if (entry.Name.EndsWith(".yml", StringComparison.OrdinalIgnoreCase))
                {
                    data.AddOrUpdate(entry.FullName, (k) => entry.Open(), (k, s) => entry.Open());
                }
            }

            return data;
        }
    }
}
