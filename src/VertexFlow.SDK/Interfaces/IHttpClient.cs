using System;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    internal interface IHttpClient : IDisposable
    {
        string BaseAddress { get; }
        
        Task<T> GetAsObjectAsync<T>(string requestUri, CancellationToken cancellationToken);
        Task PostAsJsonAsync<T>(string requestUri, T data, CancellationToken cancellationToken);
        Task PutAsJsonAsync<T>(string requestUri, T data, CancellationToken cancellationToken);
        Task DeleteAsync(string requestUri, CancellationToken cancellationToken);
    }
}