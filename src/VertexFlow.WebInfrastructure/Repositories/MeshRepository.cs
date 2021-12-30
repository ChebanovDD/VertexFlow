using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Cosmos;
using VertexFlow.WebApplication.Enums;
using VertexFlow.WebApplication.Interfaces.Repositories;

namespace VertexFlow.WebInfrastructure.Repositories
{
    internal class MeshRepository : IMeshRepository
    {
        // private readonly Container _meshContainer;
        private readonly ProjectRepository _projectRepository;
        private readonly BlobContainerClient _blobContainerClient;

        public MeshRepository(Container meshContainer, ProjectRepository projectRepository,
            BlobServiceClient blobServiceClient)
        {
            // _meshContainer = meshContainer;
            _projectRepository = projectRepository;
            _blobContainerClient = blobServiceClient.GetBlobContainerClient("meshes");
        }

        public async Task AddAsync(string projectName, string meshId, Stream meshData, CancellationToken token)
        {
            var meshGuid = await _projectRepository.AddMeshToProjectAsync(projectName, meshId, token)
                .ConfigureAwait(false);
            
            var blobClient = _blobContainerClient.GetBlobClient(meshGuid);
            await blobClient.UploadAsync(meshData, token).ConfigureAwait(false);
            
            // await _meshContainer
            //     .CreateItemStreamAsync(meshData, new PartitionKey(meshGuid), cancellationToken: token)
            //     .ConfigureAwait(false);
        }

        public async Task<Stream> GetAsync(string projectName, string meshId, CancellationToken token)
        {
            var meshGuid = await _projectRepository.GetMeshGuidAsync(projectName, meshId, token).ConfigureAwait(false);

            var blobClient = _blobContainerClient.GetBlobClient(meshGuid);
            var downloadedBlob = await blobClient.DownloadAsync(token).ConfigureAwait(false);

            return downloadedBlob.Value.Content;

            // var response = await _meshContainer
            //     .ReadItemStreamAsync(meshGuid, new PartitionKey(meshGuid), cancellationToken: token)
            //     .ConfigureAwait(false);
            //
            // return response.Content;
        }

        public async Task<MeshStatusCode> UpdateAsync(string projectName, string meshId, Stream meshData,
            CancellationToken token)
        {
            var meshGuid = await _projectRepository.GetOrCreateMeshGuidAsync(projectName, meshId, token)
                .ConfigureAwait(false);

            var blobClient = _blobContainerClient.GetBlobClient(meshGuid);
            var isBlobExists = await blobClient.ExistsAsync(token).ConfigureAwait(false);

            await blobClient.UploadAsync(meshData, true, token).ConfigureAwait(false);

            return isBlobExists ? MeshStatusCode.Updated : MeshStatusCode.Created;

            // var response = await _meshContainer
            //     .UpsertItemStreamAsync(meshData, new PartitionKey(meshGuid), cancellationToken: token)
            //     .ConfigureAwait(false);

            // return response.StatusCode == HttpStatusCode.Created ? MeshStatusCode.Created : MeshStatusCode.Updated;
        }

        public async Task DeleteAsync(string projectName, string meshId, CancellationToken token)
        {
            var meshGuid = await _projectRepository.DeleteMeshFromProjectAsync(projectName, meshId, token)
                .ConfigureAwait(false);

            var blobClient = _blobContainerClient.GetBlobClient(meshGuid);
            await blobClient.DeleteAsync(DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: token)
                .ConfigureAwait(false);

            // await _meshContainer
            //     .DeleteItemStreamAsync(meshGuid, new PartitionKey(meshGuid), cancellationToken: token)
            //     .ConfigureAwait(false);
        }
    }
}