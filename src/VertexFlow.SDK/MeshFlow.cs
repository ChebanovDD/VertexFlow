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
        
        public async Task SendAsync(TMeshData mesh)
        {
            await _meshesApi.Create<TMeshData>(mesh);
        }

        public async Task UpdateAsync(string meshId, TMeshData mesh)
        {
            await _meshesApi.UpdateAsync(meshId, mesh);
        }
    }
}