using System.Collections.Generic;
using System.Net;
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
            await _dbContainer
                .CreateItemAsync<MeshDto>(mesh.ToDto(), new PartitionKey(mesh.Id))
                .ConfigureAwait(false);
        }

        public async Task<Mesh> GetAsync(string meshId)
        {
            var meshDto = await _dbContainer
                .ReadItemAsync<MeshDto>(meshId, new PartitionKey(meshId))
                .ConfigureAwait(false);
            
            return meshDto?.Resource.ToMesh();
        }

        public async IAsyncEnumerable<Mesh> GetAllAsync()
        {
            using var query = _dbContainer.GetItemLinqQueryable<MeshDto>().ToFeedIterator();

            while (query.HasMoreResults)
            {
                foreach (var meshDto in await query.ReadNextAsync().ConfigureAwait(false))
                {
                    yield return meshDto.ToMesh();
                }
            }
        }

        public async Task<HttpStatusCode> UpdateAsync(string meshId, Mesh mesh)
        {
            var response = await _dbContainer
                .UpsertItemAsync<MeshDto>(mesh.ToDto(), new PartitionKey(meshId))
                .ConfigureAwait(false);
            
            return response.StatusCode;
        }

        public async Task DeleteAsync(string meshId)
        {
            await _dbContainer.DeleteItemAsync<MeshDto>(meshId, new PartitionKey(meshId)).ConfigureAwait(false);
        }
    }
}