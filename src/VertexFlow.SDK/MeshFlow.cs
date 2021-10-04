using System.Threading;
using System.Threading.Tasks;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK
{
    internal class MeshFlow<TMeshData> : IMeshFlow<TMeshData>
    {
        private readonly IMeshApi _meshesApi;

        public MeshFlow(IMeshApi meshesApi)
        {
            _meshesApi = meshesApi;
        }

        public async Task SendAsync(TMeshData mesh, CancellationToken cancellationToken = default)
        {
            await _meshesApi.Create<TMeshData>(mesh, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(string meshId, TMeshData mesh, CancellationToken cancellationToken = default)
        {
            await _meshesApi.UpdateAsync(meshId, mesh, cancellationToken).ConfigureAwait(false);
        }
    }
}