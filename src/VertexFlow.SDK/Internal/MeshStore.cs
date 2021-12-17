using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK.Internal
{
    internal class MeshStore<TMeshData> : IMeshStore<TMeshData>
    {
        private readonly IMeshApi _meshApi;

        public MeshStore(string projectName, IMeshApi meshApi)
        {
            _meshApi = meshApi;
            ProjectName = projectName;
        }

        public string ProjectName { get; set; }
        
        public async Task<TMeshData> GetAsync(string meshId, CancellationToken cancellationToken = default)
        {
            return await _meshApi.GetAsync<TMeshData>(ProjectName, meshId, cancellationToken).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TMeshData>> GetAllAsync()
        {
            return await _meshApi.GetAllAsync<TMeshData>(ProjectName, CancellationToken.None).ConfigureAwait(false);
        }

        public async Task DeleteAsync(string meshId, CancellationToken cancellationToken = default)
        {
            await _meshApi.DeleteAsync(ProjectName, meshId, cancellationToken).ConfigureAwait(false);
        }
    }
}