using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.WebApplication.Enums;
using VertexFlow.WebApplication.Interfaces;
using VertexFlow.WebApplication.Interfaces.Repositories;
using VertexFlow.WebApplication.Interfaces.Services;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebApplication.Services
{
    internal class MeshService : IMeshService
    {
        private readonly IMeshRepository _meshRepository;
        private readonly IMeshNotifier _meshNotifier;

        public MeshService(IMeshRepository meshRepository, IMeshNotifier meshNotifier)
        {
            _meshRepository = meshRepository;
            _meshNotifier = meshNotifier;
        }

        public async Task AddAsync(string projectName, Mesh mesh, CancellationToken cancellationToken)
        {
            await _meshRepository.AddAsync(projectName, mesh, cancellationToken).ConfigureAwait(false);
            await _meshNotifier.Created(projectName, mesh.Id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Mesh> GetAsync(string projectName, string meshId, CancellationToken cancellationToken)
        {
            return await _meshRepository.GetAsync(projectName, meshId, cancellationToken).ConfigureAwait(false);
        }

        public IAsyncEnumerable<Mesh> GetAllAsync(string projectName, CancellationToken cancellationToken)
        {
            return _meshRepository.GetAllAsync(projectName, cancellationToken);
        }

        public async Task UpdateAsync(string projectName, Mesh mesh, CancellationToken cancellationToken)
        {
            var response = await _meshRepository.UpdateAsync(projectName, mesh, cancellationToken)
                .ConfigureAwait(false);
            
            if (response == MeshStatusCode.Created)
            {
                await _meshNotifier.Created(projectName, mesh.Id, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _meshNotifier.Updated(projectName, mesh.Id, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string projectName, string meshId, CancellationToken cancellationToken)
        {
            await _meshRepository.DeleteAsync(projectName, meshId, cancellationToken).ConfigureAwait(false);
        }
    }
}