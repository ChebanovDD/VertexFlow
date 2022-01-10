using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ProtoBuf;
using VertexFlow.Contracts.Protos;
using VertexFlow.Core.Interfaces;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK.Internal.Services
{
    internal class MeshApiService : IMeshApi
    {
        private const string MeshesUri = "api/meshes/{0}";
        private const string ProjectsUri = "api/projects/{0}";

        private readonly IHttpClient _httpClient;
        private readonly IMeshMapper _meshMapper;

        public MeshApiService(IHttpClient httpClient, IMeshMapper meshMapper)
        {
            _httpClient = httpClient;
            _meshMapper = meshMapper;
        }

        public async Task Add<TMeshData>(string projectName, TMeshData meshData, CancellationToken cancellationToken)
            where TMeshData : IMeshData
        {
            using (var stream = CreateMeshStream(meshData))
            {
                await _httpClient
                    .PostAsStreamAsync($"{GetUri(projectName)}/{meshData.Id}", stream, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task<TMeshData> GetAsync<TMeshData>(string projectName, string meshId,
            CancellationToken cancellationToken)
        {
            return await GetMeshAsync<TMeshData>(GetUri(projectName), meshId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TMeshData>> GetAllAsync<TMeshData>(string projectName,
            CancellationToken cancellationToken)
        {
            // TODO: Move to ProjectApiService.
            var baseUrl = string.Format(ProjectsUri, projectName);
            var meshIds = await _httpClient
                .GetAsObjectAsync<IEnumerable<string>>($"{baseUrl}/meshIds", cancellationToken).ConfigureAwait(false);

            return await Task
                .WhenAll(meshIds.Select(meshId => GetMeshAsync<TMeshData>(GetUri(projectName), meshId, cancellationToken)))
                .ConfigureAwait(false);
        }

        public async Task UpdateAsync<TMeshData>(string projectName, TMeshData meshData,
            CancellationToken cancellationToken) where TMeshData : IMeshData
        {
            using (var stream = CreateMeshStream(meshData))
            {
                await _httpClient
                    .PutAsStreamAsync($"{GetUri(projectName)}/{meshData.Id}", stream, cancellationToken)
                    .ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(string projectName, string meshId, CancellationToken cancellationToken)
        {
            await _httpClient.DeleteAsync($"{GetUri(projectName)}/{meshId}", cancellationToken).ConfigureAwait(false);
        }

        private string GetUri(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            return string.Format(MeshesUri, projectName);
        }

        private Stream CreateMeshStream<T>(T meshData)
        {
            var stream = new MemoryStream();
            
            try
            {
                var meshDataProto = _meshMapper.From(meshData);
                Serializer.Serialize(stream, meshDataProto);
                stream.Seek(0, SeekOrigin.Begin);
            }
            catch
            {
                stream.Dispose();
            }

            return stream;
        }
        
        // TODO: Change architecture.
        private async Task<TMeshData> GetMeshAsync<TMeshData>(string baseUrl, string meshId,
            CancellationToken cancellationToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/{meshId}"))
            using (var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    var meshDataProto = Serializer.Deserialize<MeshDataProto>(stream);
                    return _meshMapper.To<TMeshData>(meshDataProto);
                }
            }
            
            // using (var stream = await _httpClient.GetAsStreamAsync($"{baseUrl}/{meshId}", cancellationToken)
            //     .ConfigureAwait(false))
            // {
            //     var mesh = Serializer.Deserialize<MeshDataProto>(stream);
            //     return _meshMapper.To<TResponse>(mesh);
            // }
        }
    }
}