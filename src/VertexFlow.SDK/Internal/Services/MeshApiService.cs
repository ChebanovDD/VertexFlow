using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK.Internal.Services
{
    internal class MeshApiService : IMeshApi
    {
        private const string MeshesUri = "api/meshes/{0}";

        private readonly IHttpClient _httpClient;

        public MeshApiService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task Add<TRequest>(string projectName, TRequest meshRequest, CancellationToken cancellationToken)
        {
            await _httpClient.PostAsJsonAsync(GetUri(projectName), meshRequest, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TResponse> GetAsync<TResponse>(string projectName, string meshId,
            CancellationToken cancellationToken)
        {
            return await _httpClient.GetAsObjectAsync<TResponse>($"{GetUri(projectName)}/{meshId}", cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<TResponse>> GetAllAsync<TResponse>(string projectName,
            CancellationToken cancellationToken)
        {
            return await _httpClient.GetAsObjectAsync<IEnumerable<TResponse>>(GetUri(projectName), cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdateAsync<TRequest>(string projectName, TRequest meshRequest,
            CancellationToken cancellationToken)
        {
            await _httpClient.PutAsJsonAsync(GetUri(projectName), meshRequest, cancellationToken).ConfigureAwait(false);
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
    }
}