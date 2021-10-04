using System;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    internal interface IMeshApi : IDisposable
    {
        string BaseAddress { get; }
        
        Task Create<TRequest>(TRequest meshRequest, CancellationToken cancellationToken);
        Task<TResponse> GetAsync<TResponse>(string meshId, CancellationToken cancellationToken);
        Task<TResponse[]> GetAllAsync<TResponse>(CancellationToken cancellationToken);
        Task UpdateAsync<TRequest>(string meshId, TRequest meshRequest, CancellationToken cancellationToken);
        Task DeleteAsync(string meshId, CancellationToken cancellationToken);
    }
}