using System.Collections.Generic;
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
            await _meshRepository.AddAsync(mesh);
        }

        public async Task<Mesh> GetAsync(int meshId)
        {
            return await _meshRepository.GetAsync(meshId);
        }

        public IAsyncEnumerable<Mesh> GetAllAsync()
        {
            return _meshRepository.GetAllAsync();
        }

        public async Task UpdateAsync(int meshId, Mesh newMesh)
        {
            await _meshRepository.UpdateAsync(meshId, newMesh);
            await _meshNotifier.Update(meshId);
        }

        public async Task DeleteAsync(int meshId)
        {
            await _meshRepository.DeleteAsync(meshId);
        }
    }
}