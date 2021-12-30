using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Internal.Interfaces
{
    internal interface IHttpClient
    {
        Task<T> GetAsObjectAsync<T>(string requestUri, CancellationToken cancellationToken);
        Task<Stream> GetAsStreamAsync(string requestUri, CancellationToken cancellationToken);
        Task PostAsJsonAsync<T>(string requestUri, T data, CancellationToken cancellationToken);
        Task PostAsStreamAsync(string requestUri, Stream stream, CancellationToken cancellationToken);
        Task PutAsJsonAsync<T>(string requestUri, T data, CancellationToken cancellationToken);
        Task PutAsStreamAsync(string requestUri, Stream stream, CancellationToken cancellationToken);
        Task DeleteAsync(string requestUri, CancellationToken cancellationToken);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken); // TODO: Remove.
    }
}