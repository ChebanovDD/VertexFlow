using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using VertexFlow.WebApplication.Interfaces.Repositories;
using VertexFlow.WebApplication.Models;
using VertexFlow.WebInfrastructure.Models;

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
            await _dbContainer.CreateItemAsync(GetMeshDto(mesh), new PartitionKey(mesh.Id));
        }

        public async Task<Mesh> GetAsync(string meshId)
        {
            var meshDto = await _dbContainer.ReadItemAsync<MeshDto>(meshId, new PartitionKey(meshId));
            return meshDto == null ? null : GetMesh(meshDto);
        }

        public async IAsyncEnumerable<Mesh> GetAllAsync()
        {
            using var query = _dbContainer.GetItemLinqQueryable<MeshDto>().ToFeedIterator();

            while (query.HasMoreResults)
            {
                foreach (var meshDto in await query.ReadNextAsync())
                {
                    yield return GetMesh(meshDto);
                }
            }
        }

        public async Task UpdateAsync(string meshId, Mesh newMesh)
        {
            await _dbContainer.UpsertItemAsync(GetMeshDto(newMesh), new PartitionKey(meshId));
        }

        public async Task DeleteAsync(string meshId)
        {
            await _dbContainer.DeleteItemAsync<MeshDto>(meshId, new PartitionKey(meshId));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private MeshDto GetMeshDto(Mesh mesh)
        {
            return new MeshDto
            {
                Id = mesh.Id,
                Triangles = mesh.Triangles,
                Vertices = mesh.Vertices,
                Normals = mesh.Normals
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Mesh GetMesh(MeshDto meshDto)
        {
            return new Mesh
            (
                meshDto.Id,
                meshDto.Triangles,
                meshDto.Vertices,
                meshDto.Normals
            );
        }
    }
}