using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Interfaces.Services
{
    public interface IMeshService
    {
        Task AddAsync(string projectName, Mesh mesh, CancellationToken cancellationToken);
        Task<Mesh> GetAsync(string projectName, string meshId, CancellationToken cancellationToken);
        IAsyncEnumerable<Mesh> GetAllAsync(string projectName, CancellationToken cancellationToken);
        Task UpdateAsync(string projectName, Mesh mesh, CancellationToken cancellationToken);
        Task DeleteAsync(string projectName, string meshId, CancellationToken cancellationToken);
    }
}