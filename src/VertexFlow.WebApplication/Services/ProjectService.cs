using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Interfaces.Repositories;
using VertexFlow.WebApplication.Interfaces.Services;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task CreateAsync(string projectName, CancellationToken cancellationToken)
        {
            await _projectRepository.CreateAsync(projectName, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Project> GetAsync(string projectName, CancellationToken cancellationToken)
        {
            return await _projectRepository.GetAsync(projectName, cancellationToken).ConfigureAwait(false);
        }

        public IAsyncEnumerable<Project> GetAllAsync(CancellationToken cancellationToken)
        {
            return _projectRepository.GetAllAsync(cancellationToken);
        }

        public IAsyncEnumerable<string> GetAllMeshIdsAsync(string projectName, CancellationToken cancellationToken)
        {
            return _projectRepository.GetAllMeshIdsAsync(projectName, cancellationToken);
        }

        public async Task DeleteAsync(string projectName, CancellationToken cancellationToken)
        {
            await _projectRepository.DeleteAsync(projectName, cancellationToken).ConfigureAwait(false);
        }
    }
}