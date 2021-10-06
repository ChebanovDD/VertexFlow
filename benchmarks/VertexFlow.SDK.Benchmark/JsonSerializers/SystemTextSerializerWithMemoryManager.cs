using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IO;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Benchmark.JsonSerializers
{
    public class SystemTextSerializerWithMemoryManager: IJsonSerializer
    {
        private const string JsonMediaType = "application/json";

        private readonly JsonSerializerOptions _serializerOptions;
        private readonly RecyclableMemoryStreamManager _memoryManager;

        public SystemTextSerializerWithMemoryManager()
        {
            _memoryManager = new RecyclableMemoryStreamManager();
            _serializerOptions = new JsonSerializerOptions
            {
                IncludeFields = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<HttpContent> SerializeAsync<T>(T data, CancellationToken cancellationToken)
        {
            if (data == null)
            {
                return null;
            }

            var memoryStream = _memoryManager.GetStream();
            await JsonSerializer.SerializeAsync(memoryStream, data, _serializerOptions, cancellationToken)
                .ConfigureAwait(false);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var httpContent = new StreamContent(memoryStream);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(JsonMediaType);

            return httpContent;
        }

        public async Task<T> DeserializeAsync<T>(HttpContent httpContent, CancellationToken cancellationToken)
        {
            var stream = await httpContent.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
            return await JsonSerializer.DeserializeAsync<T>(stream, _serializerOptions, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}