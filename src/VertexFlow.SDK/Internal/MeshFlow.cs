using System.Threading;
using System.Threading.Tasks;
using VertexFlow.Core.Interfaces;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK.Internal
{
    internal class MeshFlow<TMeshData> : IMeshFlow<TMeshData> where TMeshData : IMeshData
    {
        private readonly IMeshApi _meshesApi;

        public MeshFlow(string projectName, IMeshApi meshesApi)
        {
            _meshesApi = meshesApi;
            ProjectName = projectName;
        }

        public string ProjectName { get; set; }

        public async Task SendAsync(TMeshData mesh, CancellationToken cancellationToken = default)
        {
            await _meshesApi.Add<TMeshData>(ProjectName, mesh, cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(TMeshData mesh, CancellationToken cancellationToken = default)
        {
            await _meshesApi.UpdateAsync(ProjectName, mesh, cancellationToken).ConfigureAwait(false);
        }
    }
}