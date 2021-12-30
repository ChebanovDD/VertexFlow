using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task CreateAsync(string projectName, CancellationToken token);
        Task<Project> GetAsync(string projectName, CancellationToken token);
        IAsyncEnumerable<Project> GetAllAsync(CancellationToken token);
        IAsyncEnumerable<string> GetAllMeshIdsAsync(string projectName, CancellationToken token);
        Task DeleteAsync(string projectName, CancellationToken token);
    }
}