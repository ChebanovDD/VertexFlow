using System.Net.Http;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Internal
{
    internal class HttpClientFacadeConfig
    {
        public HttpClient HttpClient { get; set; }
        public IJsonSerializer JsonSerializer { get; set; }
    }
}