using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Interfaces.Services
{
    public interface IProjectService
    {
        Task CreateAsync(string projectName, CancellationToken cancellationToken);
        Task<Project> GetAsync(string projectName, CancellationToken cancellationToken);
        IAsyncEnumerable<Project> GetAllAsync(CancellationToken cancellationToken);
        IAsyncEnumerable<string> GetAllMeshIdsAsync(string projectName, CancellationToken cancellationToken);
        Task DeleteAsync(string projectName, CancellationToken cancellationToken);
    }
}