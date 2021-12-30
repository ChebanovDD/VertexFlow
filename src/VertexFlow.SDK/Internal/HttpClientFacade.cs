using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK.Internal
{
    internal class HttpClientFacade : IHttpClient
    {
        private const string StreamMediaType = "application/octet-stream";
        
        private readonly HttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;
        
        public HttpClientFacade(HttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
        }

        public async Task<T> GetAsObjectAsync<T>(string requestUri, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                await EnsureSuccessStatusCode(response).ConfigureAwait(false);
                
                return await _jsonSerializer.DeserializeAsync<T>(response.Content, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task<Stream> GetAsStreamAsync(string requestUri, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                await EnsureSuccessStatusCode(response).ConfigureAwait(false);

                return await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            }
        }

        public async Task PostAsJsonAsync<T>(string requestUri, T data, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
            {
                await SendAsJsonAsync(request, data, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task PostAsStreamAsync(string requestUri, Stream stream, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
            {
                await SendAsStreamAsync(request, stream, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task PutAsJsonAsync<T>(string requestUri, T data, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, requestUri))
            {
                await SendAsJsonAsync(request, data, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task PutAsStreamAsync(string requestUri, Stream stream, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Put, requestUri))
            {
                await SendAsStreamAsync(request, stream, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string requestUri, CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Delete, requestUri))
            using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                await EnsureSuccessStatusCode(response).ConfigureAwait(false);
            }
        }

        private async Task SendAsJsonAsync<T>(HttpRequestMessage request, T data, CancellationToken cancellationToken)
        {
            using (var httpContent = await _jsonSerializer.SerializeAsync(data, cancellationToken).ConfigureAwait(false))
            {
                request.Content = httpContent;
                using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    await EnsureSuccessStatusCode(response).ConfigureAwait(false);
                }
            }
        }
        
        private async Task SendAsStreamAsync(HttpRequestMessage request, Stream stream, CancellationToken cancellationToken)
        {
            using (var httpContent = CreateStreamContent(stream))
            {
                request.Content = httpContent;
                using (var response = await SendAsync(request, cancellationToken).ConfigureAwait(false))
                {
                    await EnsureSuccessStatusCode(response).ConfigureAwait(false);
                }
            }
        }

        // TODO: Make private.
        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = await _httpClient
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            
            return response;
        }

        private StreamContent CreateStreamContent(Stream stream)
        {
            var httpContent = new StreamContent(stream);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(StreamMediaType);

            return httpContent;
        }

        private async ValueTask EnsureSuccessStatusCode(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return;
            }

            if (response.Content == null)
            {
                response.EnsureSuccessStatusCode();
            }
            else
            {
                throw new HttpRequestException(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }
    }
}