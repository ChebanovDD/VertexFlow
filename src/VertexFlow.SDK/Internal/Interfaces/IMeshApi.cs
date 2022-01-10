using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.Core.Interfaces;

namespace VertexFlow.SDK.Internal.Interfaces
{
    internal interface IMeshApi
    {
        Task Add<TMeshData>(string projectName, TMeshData meshData, CancellationToken cancellationToken) where TMeshData : IMeshData;
        Task<TMeshData> GetAsync<TMeshData>(string projectName, string meshId, CancellationToken cancellationToken);
        Task<IEnumerable<TMeshData>> GetAllAsync<TMeshData>(string projectName, CancellationToken cancellationToken);
        Task UpdateAsync<TMeshData>(string projectName, TMeshData meshData, CancellationToken cancellationToken) where TMeshData : IMeshData;
        Task DeleteAsync(string projectName, string meshId, CancellationToken cancellationToken);
    }
}