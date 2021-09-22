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
        
        public async Task<TMeshData> GetAsync(string meshId)
        {
            return await _meshApi.GetAsync<TMeshData>(meshId);
        }

        public async Task<TMeshData[]> GetAllAsync()
        {
            return await _meshApi.GetAllAsync<TMeshData>();
        }

        public async Task DeleteAsync(string meshId)
        {
            await _meshApi.DeleteAsync(meshId);
        }
    }
}