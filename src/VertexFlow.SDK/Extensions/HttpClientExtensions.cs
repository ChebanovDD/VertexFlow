using System.Net.Http;

namespace VertexFlow.SDK.Extensions
{
    internal static class HttpClientExtensions
    {
        public static HttpClient AddHeader(this HttpClient client, string name, string value)
        {
            client.DefaultRequestHeaders.Add(name, value);
            return client;
        }
    }
}