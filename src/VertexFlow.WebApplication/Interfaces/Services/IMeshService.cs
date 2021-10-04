using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Interfaces.Services
{
    public interface IMeshService
    {
        Task AddAsync(Mesh mesh, CancellationToken cancellationToken);
        Task<Mesh> GetAsync(string meshId, CancellationToken cancellationToken);
        IAsyncEnumerable<Mesh> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(string meshId, Mesh newMesh, CancellationToken cancellationToken);
        Task DeleteAsync(string meshId, CancellationToken cancellationToken);
    }
}