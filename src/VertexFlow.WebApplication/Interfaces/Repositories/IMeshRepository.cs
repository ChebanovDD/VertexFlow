using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Enums;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Interfaces.Repositories
{
    public interface IMeshRepository
    {
        Task AddAsync(Mesh mesh, CancellationToken token);
        Task<Mesh> GetAsync(string meshId, CancellationToken token);
        IAsyncEnumerable<Mesh> GetAllAsync(CancellationToken token);
        Task<MeshStatusCode> UpdateAsync(string meshId, Mesh newMesh, CancellationToken token);
        Task DeleteAsync(string meshId, CancellationToken token);
    }
}