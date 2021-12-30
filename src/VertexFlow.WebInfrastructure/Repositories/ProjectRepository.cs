using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using VertexFlow.WebApplication.Interfaces.Repositories;
using VertexFlow.WebApplication.Models;
using VertexFlow.WebInfrastructure.DTOs;
using VertexFlow.WebInfrastructure.Extensions;

namespace VertexFlow.WebInfrastructure.Repositories
{
    internal class ProjectRepository : IProjectRepository
    {
        private readonly Container _projectContainer;
        
        public ProjectRepository(Container projectContainer)
        {
            _projectContainer = projectContainer;
        }

        public async Task CreateAsync(string projectName, CancellationToken token)
        {
            await AddProjectDtoAsync(new ProjectDto { Name = projectName, MeshIds = new Dictionary<string, string>() },
                token).ConfigureAwait(false);
        }

        public async Task<Project> GetAsync(string projectName, CancellationToken token)
        {
            var projectDto = await GetProjectDtoAsync(projectName, token).ConfigureAwait(false);
            
            return projectDto.ToProject();
        }

        public async IAsyncEnumerable<Project> GetAllAsync([EnumeratorCancellation] CancellationToken token)
        {
            using var query = _projectContainer.GetItemLinqQueryable<ProjectDto>().ToFeedIterator();

            while (query.HasMoreResults)
            {
                foreach (var projectDto in await query.ReadNextAsync(token).ConfigureAwait(false))
                {
                    yield return projectDto.ToProject();
                }
            }
        }

        public async IAsyncEnumerable<string> GetAllMeshIdsAsync(string projectName,
            [EnumeratorCancellation] CancellationToken token)
        {
            ProjectDto projectDto = null;
            
            try
            {
                projectDto = await GetProjectDtoAsync(projectName, token).ConfigureAwait(false);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
            }

            if (projectDto == null)
            {
                yield break;
            }
            
            foreach (var pair in projectDto.MeshIds)
            {
                yield return pair.Key;
            }
        }

        public async Task DeleteAsync(string projectName, CancellationToken token)
        {
            // TODO: Remove all meshes.
            
            await _projectContainer
                .DeleteItemAsync<ProjectDto>(projectName, new PartitionKey(projectName), cancellationToken: token)
                .ConfigureAwait(false);
        }

        public async Task<string> AddMeshToProjectAsync(string projectName, string meshId, CancellationToken token)
        {
            var projectDto = await GetProjectDtoAsync(projectName, token).ConfigureAwait(false);

            return await AddMeshToProjectDtoAsync(projectDto, meshId, token).ConfigureAwait(false);
        }

        public async Task<string> GetMeshGuidAsync(string projectName, string meshId, CancellationToken token)
        {
            var projectDto = await GetProjectDtoAsync(projectName, token).ConfigureAwait(false);

            return projectDto.MeshIds[meshId];
        }

        public async Task<string> GetOrCreateMeshGuidAsync(string projectName, string meshId, CancellationToken token)
        {
            ProjectDto projectDto = null;

            try
            {
                projectDto = await GetProjectDtoAsync(projectName, token).ConfigureAwait(false);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
            }

            if (projectDto == null)
            {
                projectDto = new ProjectDto
                {
                    Name = projectName,
                    MeshIds = new Dictionary<string, string>
                    {
                        { meshId, Guid.NewGuid().ToString() }
                    }
                };

                await AddProjectDtoAsync(projectDto, token).ConfigureAwait(false);
            }

            if (projectDto.MeshIds.TryGetValue(meshId, out var meshGuid))
            {
                return meshGuid;
            }

            return await AddMeshToProjectDtoAsync(projectDto, meshId, token).ConfigureAwait(false);
        }

        public async Task<string> DeleteMeshFromProjectAsync(string projectName, string meshId, CancellationToken token)
        {
            var projectDto = await GetProjectDtoAsync(projectName, token).ConfigureAwait(false);
            
            var meshGuid = projectDto.MeshIds[meshId];
            projectDto.MeshIds.Remove(meshId);

            await ReplaceProjectDtoAsync(projectDto, token).ConfigureAwait(false);

            return meshGuid;
        }

        private async ValueTask AddProjectDtoAsync(ProjectDto projectDto, CancellationToken token)
        {
            await _projectContainer
                .CreateItemAsync<ProjectDto>(projectDto, new PartitionKey(projectDto.Name), cancellationToken: token)
                .ConfigureAwait(false);
        }

        private async ValueTask<ProjectDto> GetProjectDtoAsync(string projectName, CancellationToken token)
        {
            var response = await _projectContainer
                .ReadItemAsync<ProjectDto>(projectName, new PartitionKey(projectName), cancellationToken: token)
                .ConfigureAwait(false);

            return response.Resource;
        }

        private async ValueTask ReplaceProjectDtoAsync(ProjectDto projectDto, CancellationToken token)
        {
            await _projectContainer
                .ReplaceItemAsync<ProjectDto>(projectDto, projectDto.Name, new PartitionKey(projectDto.Name),
                    cancellationToken: token)
                .ConfigureAwait(false);
        }

        private async ValueTask<string> AddMeshToProjectDtoAsync(ProjectDto projectDto, string meshId,
            CancellationToken token)
        {
            var meshGuid = Guid.NewGuid().ToString();
            projectDto.MeshIds.Add(meshId, meshGuid);

            await ReplaceProjectDtoAsync(projectDto, token).ConfigureAwait(false);

            return meshGuid;
        }
    }
}