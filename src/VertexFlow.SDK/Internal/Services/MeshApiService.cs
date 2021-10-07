using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK.Internal.Services
{
    internal class MeshApiService : IMeshApi
    {
        private const string MeshesUri = "api/meshes";

        private readonly IHttpClient _httpClient;
        
        public string BaseAddress => _httpClient.BaseAddress;
        
        public MeshApiService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<IEnumerable<TResponse>> GetAllAsync<TResponse>(CancellationToken cancellationToken)
        {
            return await _httpClient.GetAsObjectAsync<IEnumerable<TResponse>>(MeshesUri, cancellationToken).ConfigureAwait(false);
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
    }
}