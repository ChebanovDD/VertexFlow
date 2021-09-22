using System.Collections.Generic;
using System.Threading.Tasks;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK
{
    internal class MeshStore<TMeshData> : IMeshStore<TMeshData>
    {
        private readonly IMeshesApi _meshesApi;
        
        public MeshStore(IMeshesApi meshesApi)
        {
            _meshesApi = meshesApi;
        }

        public async Task<TMeshData> GetAsync(string meshId)
        {
            var response = await _meshesApi.GetAsync<TMeshData>(meshId);
            return response.Content;
        }
        
        public async IAsyncEnumerable<TMeshData> GetAllAsync()
        {
            var response = await _meshesApi.GetAllAsync<TMeshData>();
            if (response.Content == null)
            {
                yield break;
            }

            var count = response.Content.Length;
            for (var i = 0; i < count; i++)
            {
                yield return response.Content[i];
            }
        }
        
        public async Task DeleteAsync(string meshId)
        {
            await _meshesApi.DeleteAsync(meshId);
        }
    }
}