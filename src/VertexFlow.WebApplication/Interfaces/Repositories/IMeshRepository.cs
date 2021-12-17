using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Enums;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Interfaces.Repositories
{
    public interface IMeshRepository
    {
        Task AddAsync(string projectName, Mesh mesh, CancellationToken token);
        Task<Mesh> GetAsync(string projectName, string meshId, CancellationToken token);
        IAsyncEnumerable<Mesh> GetAllAsync(string projectName, CancellationToken token);
        Task<MeshStatusCode> UpdateAsync(string projectName, Mesh newMesh, CancellationToken token);
        Task DeleteAsync(string projectName, string meshId, CancellationToken token);
    }
}