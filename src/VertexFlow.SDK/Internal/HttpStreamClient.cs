using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Internal
{
    internal class HttpStreamClient : IHttpClient
    {
        private const string JsonMediaType = "application/json";
        
        private readonly HttpClient _httpClient;
        private readonly UTF8Encoding _utf8Encoding;
        private readonly JsonSerializer _jsonSerializer;

        public string BaseAddress => _httpClient.BaseAddress?.OriginalString;
        
        public HttpStreamClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _utf8Encoding = new UTF8Encoding(false);
            _jsonSerializer = new JsonSerializer();
        }

        public async Task<T> GetAsObjectAsync<T>(string requestUri, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    return DeserializeJsonFromStream<T>(stream);
                }
            }
        }

        public async Task PostAsJsonAsync<T>(string requestUri, T data, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
            {
                await SendAsJsonAsync(request, data, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task PutAsJsonAsync<T>(string requestUri, T data, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, requestUri))
            {
                await SendAsJsonAsync(request, data, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, requestUri))
            {
                using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        private async Task SendAsJsonAsync<T>(HttpRequestMessage request, T data, CancellationToken cancellationToken)
        {
            using (var streamContent = GetStreamContent(data))
            {
                request.Content = streamContent;
                using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        private async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            
            return response;
        }

        private HttpContent GetStreamContent(object data)
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

            return httpContent;
        }
        
        private void SerializeJsonIntoStream(object data, Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream, _utf8Encoding, 1024, true))
            {
                using (var jsonTextWriter = new JsonTextWriter(streamWriter) { Formatting = Formatting.None })
                {
                    _jsonSerializer.Serialize(jsonTextWriter, data);
                    jsonTextWriter.Flush();
                }
            }
        }
        
        private T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
            {
                return default;
            }

            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return _jsonSerializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}