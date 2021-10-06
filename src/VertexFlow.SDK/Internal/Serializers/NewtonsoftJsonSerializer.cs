using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Internal.Serializers
{
    internal class NewtonsoftJsonSerializer : IJsonSerializer
    {
        private const string JsonMediaType = "application/json";
        
        private readonly UTF8Encoding _utf8Encoding;
        private readonly JsonSerializer _jsonSerializer;
        
        public NewtonsoftJsonSerializer()
        {
            _utf8Encoding = new UTF8Encoding(false);
            _jsonSerializer = new JsonSerializer();
        }
        
        public Task<HttpContent> SerializeAsync<T>(T data, CancellationToken cancellationToken)
        {
            if (data == null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            SerializeJsonIntoStream(data, memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var httpContent = new StreamContent(memoryStream);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(JsonMediaType);

            return Task.FromResult<HttpContent>(httpContent);
        }

        public async Task<T> DeserializeAsync<T>(HttpContent httpContent, CancellationToken cancellationToken)
        {
            var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
            if (stream == null || stream.CanRead == false)
            {
                return default;
            }

            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return _jsonSerializer.Deserialize<T>(jsonTextReader);
            }
        }
        
        private void SerializeJsonIntoStream(object data, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream, _utf8Encoding, 1024, true))
            using (var jsonTextWriter = new JsonTextWriter(streamWriter) { Formatting = Formatting.None })
            {
                _jsonSerializer.Serialize(jsonTextWriter, data);
                jsonTextWriter.Flush();
            }
        }
    }
}