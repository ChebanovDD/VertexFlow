using System.Net.Http;
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

        public MeshApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Create<TRequest>(TRequest meshRequest)
        {
            await _httpClient.PostAsJsonAsync(MeshesUri, meshRequest).ConfigureAwait(false);
        }

        public async Task<TResponse> GetAsync<TResponse>(string meshId)
        {
            return await _httpClient.GetAsObjectAsync<TResponse>($"{MeshesUri}/{meshId}").ConfigureAwait(false);
        }

        public async Task<TResponse[]> GetAllAsync<TResponse>()
        {
            return await _httpClient.GetAsObjectAsync<TResponse[]>(MeshesUri).ConfigureAwait(false);
        }

        public async Task UpdateAsync<TRequest>(string meshId, TRequest meshRequest)
        {
            await _httpClient.PutAsJsonAsync($"{MeshesUri}/{meshId}", meshRequest).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string meshId)
        {
            await _httpClient.DeleteAsync($"{MeshesUri}/{meshId}").ConfigureAwait(false);
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}