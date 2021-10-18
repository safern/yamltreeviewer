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
        private AsyncLazy<ConcurrentDictionary<string, Stream>> _zipStreamData;
        private ZipArchive _zipArchive;

        public ZipStreamProvider(Stream zipStream)
        {
            _zipStream = zipStream;
            _zipArchive = new ZipArchive(_zipStream, ZipArchiveMode.Read, leaveOpen: true);
            _zipStreamData = new AsyncLazy<ConcurrentDictionary<string, Stream>>(InitZipStreamData);
        }

        public async Task<Stream> GetStreamFromPathAsync(string path, CancellationToken cancellationToken = default)
        {
            ConcurrentDictionary<string, Stream> dictionary = await _zipStreamData.Value;

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
                ConcurrentDictionary<string, Stream> dic = await _zipStreamData.Value;
                foreach (Stream stream in dic.Values)
                    await stream.DisposeAsync();

                dic.Clear();
                _zipStreamData = null!;
            }
        }

        private Task<ConcurrentDictionary<string, Stream>> InitZipStreamData()
        {
            return Task.Run(() =>
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
            });
        }

        private class AsyncLazy<T> : Lazy<Task<T>>
        {
            public AsyncLazy(Func<T> valueFactory) :
                base(() => Task.Factory.StartNew(valueFactory)) { }

            public AsyncLazy(Func<Task<T>> taskFactory) :
                base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap()) { }
        }
    }
}
