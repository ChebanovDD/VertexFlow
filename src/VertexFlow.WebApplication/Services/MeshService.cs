using System.Collections.Generic;
using System.Net;
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

        public async Task AddAsync(Mesh mesh)
        {   
            await _meshRepository.AddAsync(mesh).ConfigureAwait(false);
            await _meshNotifier.Created(mesh.Id).ConfigureAwait(false);
        }

        public async Task<Mesh> GetAsync(string meshId)
        {
            return await _meshRepository.GetAsync(meshId).ConfigureAwait(false);
        }

        public IAsyncEnumerable<Mesh> GetAllAsync()
        {
            return _meshRepository.GetAllAsync();
        }

        public async Task UpdateAsync(string meshId, Mesh newMesh)
        {
            var response = await _meshRepository.UpdateAsync(meshId, newMesh).ConfigureAwait(false);
            if (response == HttpStatusCode.Created)
            {
                await _meshNotifier.Created(meshId).ConfigureAwait(false);
            }
            else
            {
                await _meshNotifier.Updated(meshId).ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string meshId)
        {
            await _meshRepository.DeleteAsync(meshId).ConfigureAwait(false);
        }
    }
}