using System;
using System.Net.Http;
using VertexFlow.Contracts.Responses;
using VertexFlow.SDK.Extensions;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal;
using VertexFlow.SDK.Internal.Services;

namespace VertexFlow.SDK
{
    public class VertexFlow : IDisposable
    {
        private readonly IMeshApi _meshesApi;
        
        public string Server => _meshesApi.BaseAddress;

        public VertexFlow(string server, string version = "1.0")
        {
            _meshesApi =
                new MeshApiService(
                    new HttpStreamClient(
                        new HttpClient { BaseAddress = new Uri(server) }.AddHeader("version", version)));
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