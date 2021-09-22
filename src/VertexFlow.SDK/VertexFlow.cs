using System;
using System.Net.Http;
using Refit;
using VertexFlow.Contracts.Responses;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK
{
    public class VertexFlow : IDisposable
    {
        private readonly IMeshesApi _meshesApi;
        private readonly HttpClient _httpClient;
        
        public string Server => _httpClient.BaseAddress?.OriginalString;
        
        public VertexFlow(string server)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(server) };
            _meshesApi = RestService.For<IMeshesApi>(_httpClient);
        }
        
        public IMeshFlow<MeshResponse> CreateMeshFlow()
        {
            return new MeshFlow<MeshResponse>(_meshesApi);
        }
        
        public IMeshFlow<TMeshData> CreateMeshFlow<TMeshData>()
        {
            return new MeshFlow<TMeshData>(_meshesApi);
        }
        
        public IMeshStore<MeshResponse> CreateMeshStore()
        {
            return new MeshStore<MeshResponse>(_meshesApi);
        }
        
        public IMeshStore<TMeshData> CreateMeshStore<TMeshData>()
        {
            return new MeshStore<TMeshData>(_meshesApi);
        }
        
        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}