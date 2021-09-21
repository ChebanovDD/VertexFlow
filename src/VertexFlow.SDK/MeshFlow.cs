using System.Threading.Tasks;
using VertexFlow.Contracts.Responses;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK
{
    internal class MeshFlow<TMeshData> : IMeshFlow<TMeshData>
    {
        private readonly IMeshesApi _meshesApi;
        
        public MeshFlow(IMeshesApi meshesApi)
        {
            _meshesApi = meshesApi;
        }
        
        public async Task SendAsync(TMeshData mesh)
        {
            await _meshesApi.Create<TMeshData, MeshResponse>(mesh);
        }

        public async Task UpdateAsync(string meshId, TMeshData mesh)
        {
            await _meshesApi.UpdateAsync(meshId, mesh);
        }
    }
}