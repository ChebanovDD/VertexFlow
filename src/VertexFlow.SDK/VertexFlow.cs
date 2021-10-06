using System;
using System.Net.Http;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK
{
    public class VertexFlow : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly MeshApiProvider _meshApiProvider;
        
        public string Server => _httpClient?.BaseAddress.OriginalString;
        
        public VertexFlow(string server, string version = "1.0")
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(server) };
            _httpClient.DefaultRequestHeaders.Add("version", version);
            _meshApiProvider = new MeshApiProvider();
        }
        
        public IMeshFlow<TMeshData> CreateMeshFlow<TMeshData>(IJsonSerializer jsonSerializer = null)
        {
            return new MeshFlow<TMeshData>(GetMeshApi(jsonSerializer));
        }
        
        public IMeshStore<TMeshData> CreateMeshStore<TMeshData>(IJsonSerializer jsonSerializer = null)
        {
            return new MeshStore<TMeshData>(GetMeshApi(jsonSerializer));
        }
        
        public void Dispose()
        {
            _httpClient?.Dispose();
            _meshApiProvider?.Dispose();
        }
        
        private IMeshApi GetMeshApi(IJsonSerializer jsonSerializer)
        {
            return _meshApiProvider.GetMeshApi(_httpClient, jsonSerializer);
        }
    }
}