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
        Task DeleteAsync(string projectName, CancellationToken token);

        Task<string> AddMeshToProjectAsync(string projectName, string meshId, CancellationToken token);
        Task<string> GetMeshGuidAsync(string projectName, string meshId, CancellationToken token);
        Task<string> GetOrCreateMeshGuidAsync(string projectName, string meshId, CancellationToken token);
        Task<string> DeleteMeshFromProjectAsync(string projectName, string meshId, CancellationToken token);
    }
}