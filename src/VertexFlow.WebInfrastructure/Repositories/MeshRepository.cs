using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using VertexFlow.WebApplication.Interfaces.Repositories;
using VertexFlow.WebApplication.Models;

namespace VertexFlow.WebInfrastructure.Repositories
{
    internal class MeshRepository : IMeshRepository
    {
        private readonly Container _dbContainer;
        
        public MeshRepository(Container dbContainer)
        {
            _dbContainer = dbContainer;
        }

        public Task AddAsync(Mesh mesh)
        {
            throw new System.NotImplementedException();
        }

        public Task<Mesh> GetAsync(int meshId)
        {
            throw new System.NotImplementedException();
        }

        public IAsyncEnumerable<Mesh> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(int meshId, Mesh newMesh)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(int meshId)
        {
            throw new System.NotImplementedException();
        }
    }
}