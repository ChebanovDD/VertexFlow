using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using VertexFlow.WebApplication.Enums;
using VertexFlow.WebApplication.Interfaces.Repositories;
using VertexFlow.WebApplication.Models;
using VertexFlow.WebInfrastructure.DTOs;
using VertexFlow.WebInfrastructure.Extensions;

namespace VertexFlow.WebInfrastructure.Repositories
{
    internal class MeshRepository : IMeshRepository
    {
        private readonly Container _meshContainer;
        private readonly ProjectRepository _projectRepository;

        public MeshRepository(Container meshContainer, ProjectRepository projectRepository)
        {
            _meshContainer = meshContainer;
            _projectRepository = projectRepository;
        }

        public async Task AddAsync(string projectName, Mesh mesh, CancellationToken token)
        {
            var meshGuid = await _projectRepository.AddMeshToProjectAsync(projectName, mesh.Id, token)
                .ConfigureAwait(false);
            
            await _meshContainer
                .CreateItemAsync<MeshDto>(mesh.ToDto(meshGuid), new PartitionKey(meshGuid), cancellationToken: token)
                .ConfigureAwait(false);
        }

        public async Task<Mesh> GetAsync(string projectName, string meshId, CancellationToken token)
        {
            var meshGuid = await _projectRepository.GetMeshGuidAsync(projectName, meshId, token).ConfigureAwait(false);
            
            var meshDto = await GetMeshDtoAsync(meshGuid, token).ConfigureAwait(false);

            return meshDto.ToMesh(meshId);
        }

        public async IAsyncEnumerable<Mesh> GetAllAsync(string projectName, [EnumeratorCancellation] CancellationToken token)
        {
            var project = await _projectRepository.GetAsync(projectName, token).ConfigureAwait(false);

            foreach (var (meshId, meshGuid) in project.MeshIds)
            {
                var meshDto = await GetMeshDtoAsync(meshGuid, token).ConfigureAwait(false);

                yield return meshDto.ToMesh(meshId);
            }
        }

        public async Task<MeshStatusCode> UpdateAsync(string projectName, Mesh mesh, CancellationToken token)
        {
            var meshGuid = await _projectRepository.GetOrCreateMeshGuidAsync(projectName, mesh.Id, token)
                .ConfigureAwait(false);

            var response = await _meshContainer
                .UpsertItemAsync<MeshDto>(mesh.ToDto(meshGuid), new PartitionKey(meshGuid), cancellationToken: token)
                .ConfigureAwait(false);

            return response.StatusCode == HttpStatusCode.Created ? MeshStatusCode.Created : MeshStatusCode.Updated;
        }

        public async Task DeleteAsync(string projectName, string meshId, CancellationToken token)
        {
            var meshGuid = await _projectRepository.DeleteMeshFromProjectAsync(projectName, meshId, token)
                .ConfigureAwait(false);

            await _meshContainer
                .DeleteItemAsync<MeshDto>(meshGuid, new PartitionKey(meshGuid), cancellationToken: token)
                .ConfigureAwait(false);
        }

        private async Task<MeshDto> GetMeshDtoAsync(string meshGuid, CancellationToken token)
        {
            var response = await _meshContainer
                .ReadItemAsync<MeshDto>(meshGuid, new PartitionKey(meshGuid), cancellationToken: token)
                .ConfigureAwait(false);

            return response.Resource;
        }
    }
}