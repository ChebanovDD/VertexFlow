using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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

        public async Task AddAsync(Mesh mesh, CancellationToken cancellationToken)
        {
            await _meshRepository.AddAsync(mesh, cancellationToken).ConfigureAwait(false);
            await _meshNotifier.Created(mesh.Id, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Mesh> GetAsync(string meshId, CancellationToken cancellationToken)
        {
            return await _meshRepository.GetAsync(meshId, cancellationToken).ConfigureAwait(false);
        }

        public IAsyncEnumerable<Mesh> GetAllAsync(CancellationToken cancellationToken)
        {
            return _meshRepository.GetAllAsync(cancellationToken);
        }

        public async Task UpdateAsync(string meshId, Mesh newMesh, CancellationToken cancellationToken)
        {
            var response = await _meshRepository.UpdateAsync(meshId, newMesh, cancellationToken).ConfigureAwait(false);
            if (response == HttpStatusCode.Created)
            {
                await _meshNotifier.Created(meshId, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                await _meshNotifier.Updated(meshId, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string meshId, CancellationToken cancellationToken)
        {
            await _meshRepository.DeleteAsync(meshId, cancellationToken).ConfigureAwait(false);
        }
    }
}