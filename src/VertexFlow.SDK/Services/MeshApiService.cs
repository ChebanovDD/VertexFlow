using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.SDK.Extensions;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Services
{
    internal class MeshApiService : IMeshApi
    {
        private const string MeshesUri = "api/meshes";

        private readonly HttpClient _httpClient;

        public string BaseAddress => _httpClient.BaseAddress?.OriginalString;

        public MeshApiService(string server, string version)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(server) };
            _httpClient.DefaultRequestHeaders.Add("version", version);
        }

        public async Task Create<TRequest>(TRequest meshRequest, CancellationToken cancellationToken)
        {
            await _httpClient.PostAsJsonAsync(MeshesUri, meshRequest, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> GetAsync<TResponse>(string meshId, CancellationToken cancellationToken)
        {
            return await _httpClient.GetAsObjectAsync<TResponse>($"{MeshesUri}/{meshId}", cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<TResponse[]> GetAllAsync<TResponse>(CancellationToken cancellationToken)
        {
            return await _httpClient.GetAsObjectAsync<TResponse[]>(MeshesUri, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync<TRequest>(string meshId, TRequest meshRequest,
            CancellationToken cancellationToken)
        {
            await _httpClient.PutAsJsonAsync($"{MeshesUri}/{meshId}", meshRequest, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteAsync(string meshId, CancellationToken cancellationToken)
        {
            await _httpClient.DeleteAsync($"{MeshesUri}/{meshId}", cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}