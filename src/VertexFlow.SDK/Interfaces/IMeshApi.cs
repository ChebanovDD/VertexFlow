using System;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    internal interface IMeshApi : IDisposable
    {
        string BaseAddress { get; }
        
        Task Create<TRequest>(TRequest meshRequest);
        Task<TResponse> GetAsync<TResponse>(string meshId);
        Task<TResponse[]> GetAllAsync<TResponse>();
        Task UpdateAsync<TRequest>(string meshId, TRequest meshRequest);
        Task DeleteAsync(string meshId);
    }
}