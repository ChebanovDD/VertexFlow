using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using VertexFlow.WebApplication.Interfaces.Repositories;
using VertexFlow.WebApplication.Models;
using VertexFlow.WebInfrastructure.DTOs;
using VertexFlow.WebInfrastructure.Extensions;

namespace VertexFlow.WebInfrastructure.Repositories
{
    internal class MeshRepository : IMeshRepository
    {
        private readonly Container _dbContainer;

        public MeshRepository(Container dbContainer)
        {
            _dbContainer = dbContainer;
        }

        public async Task AddAsync(Mesh mesh)
        {
            await _dbContainer.CreateItemAsync<MeshDto>(mesh.ToDto(), new PartitionKey(mesh.Id));
        }

        public async Task<Mesh> GetAsync(string meshId)
        {
            var meshDto = await _dbContainer.ReadItemAsync<MeshDto>(meshId, new PartitionKey(meshId));
            return meshDto?.Resource.ToMesh();
        }

        public async IAsyncEnumerable<Mesh> GetAllAsync()
        {
            using var query = _dbContainer.GetItemLinqQueryable<MeshDto>().ToFeedIterator();

            while (query.HasMoreResults)
            {
                foreach (var meshDto in await query.ReadNextAsync())
                {
                    yield return meshDto.ToMesh();
                }
            }
        }

        public async Task UpdateAsync(string meshId, Mesh newMesh)
        {
            await _dbContainer.UpsertItemAsync<MeshDto>(newMesh.ToDto(), new PartitionKey(meshId));
        }

        public async Task DeleteAsync(string meshId)
        {
            await _dbContainer.DeleteItemAsync<MeshDto>(meshId, new PartitionKey(meshId));
        }
    }
}