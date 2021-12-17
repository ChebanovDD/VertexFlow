using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Internal.Interfaces
{
    internal interface IMeshApi
    {
        Task Add<TRequest>(string projectName, TRequest meshRequest, CancellationToken cancellationToken);
        Task<TResponse> GetAsync<TResponse>(string projectName, string meshId, CancellationToken cancellationToken);
        Task<IEnumerable<TResponse>> GetAllAsync<TResponse>(string projectName, CancellationToken cancellationToken);
        Task UpdateAsync<TRequest>(string projectName, TRequest meshRequest, CancellationToken cancellationToken);
        Task DeleteAsync(string projectName, string meshId, CancellationToken cancellationToken);
    }
}