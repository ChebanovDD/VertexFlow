using System;
using System.Net.Http;
using VertexFlow.Contracts.Responses;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal;
using VertexFlow.SDK.Internal.Interfaces;
using VertexFlow.SDK.Internal.Serializers;
using VertexFlow.SDK.Internal.Services;

namespace VertexFlow.SDK
{
    public class VertexFlow : IDisposable
    {
        private readonly IMeshApi _meshesApi;
        
        public string Server => _meshesApi.BaseAddress;
        
        public VertexFlow(string server, string version = "1.0", IJsonSerializer jsonSerializer = null)
        {
            _meshesApi = new MeshApiService(new HttpClientFacade(config =>
            {
                config.HttpClient = new HttpClient { BaseAddress = new Uri(server) };
                config.HttpClient.DefaultRequestHeaders.Add("version", version);
                config.JsonSerializer = jsonSerializer ?? new NewtonsoftJsonSerializer();
            }));
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
            _meshesApi?.Dispose();
        }
    }
}