using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using VertexFlow.WebApplication.Enums;
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

        public async Task AddAsync(Mesh mesh, CancellationToken token)
        {
            await _dbContainer
                .CreateItemAsync<MeshDto>(mesh.ToDto(), new PartitionKey(mesh.Id), cancellationToken: token)
                .ConfigureAwait(false);
        }

        public async Task<Mesh> GetAsync(string meshId, CancellationToken token)
        {
            var meshDto = await _dbContainer
                .ReadItemAsync<MeshDto>(meshId, new PartitionKey(meshId), cancellationToken: token)
                .ConfigureAwait(false);

            return meshDto?.Resource.ToMesh();
        }

        public async IAsyncEnumerable<Mesh> GetAllAsync([EnumeratorCancellation] CancellationToken token)
        {
            using var query = _dbContainer.GetItemLinqQueryable<MeshDto>().ToFeedIterator();

            while (query.HasMoreResults)
            {
                foreach (var meshDto in await query.ReadNextAsync(token).ConfigureAwait(false))
                {
                    yield return meshDto.ToMesh();
                }
            }
        }

        public async Task<MeshStatusCode> UpdateAsync(string meshId, Mesh mesh, CancellationToken token)
        {
            var response = await _dbContainer
                .UpsertItemAsync<MeshDto>(mesh.ToDto(), new PartitionKey(meshId), cancellationToken: token)
                .ConfigureAwait(false);

            return response.StatusCode == HttpStatusCode.Created ? MeshStatusCode.Created : MeshStatusCode.Updated;
        }

        public async Task DeleteAsync(string meshId, CancellationToken token)
        {
            await _dbContainer.DeleteItemAsync<MeshDto>(meshId, new PartitionKey(meshId), cancellationToken: token)
                .ConfigureAwait(false);
        }
    }
}