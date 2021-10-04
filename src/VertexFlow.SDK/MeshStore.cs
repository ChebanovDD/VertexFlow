using System.Threading;
using System.Threading.Tasks;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK
{
    internal class MeshStore<TMeshData> : IMeshStore<TMeshData>
    {
        private readonly IMeshApi _meshApi;

        public MeshStore(IMeshApi meshApi)
        {
            _meshApi = meshApi;
        }

        public async Task<TMeshData> GetAsync(string meshId, CancellationToken cancellationToken = default)
        {
            return await _meshApi.GetAsync<TMeshData>(meshId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TMeshData[]> GetAllAsync()
        {
            return await _meshApi.GetAllAsync<TMeshData>(CancellationToken.None).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string meshId, CancellationToken cancellationToken = default)
        {
            await _meshApi.DeleteAsync(meshId, cancellationToken).ConfigureAwait(false);
        }
    }
}