using System;
using System.Collections.Generic;
using System.Net.Http;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal.Interfaces;
using VertexFlow.SDK.Internal.Serializers;
using VertexFlow.SDK.Internal.Services;

namespace VertexFlow.SDK.Internal
{
    internal class MeshApiProvider : IDisposable
    {
        private Dictionary<Type, IMeshApi> _meshApis;

        public MeshApiProvider()
        {
            _meshApis = new Dictionary<Type, IMeshApi>();
        }

        public IMeshApi GetMeshApi(HttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            return jsonSerializer == null
                ? GetDefaultMeshApi(httpClient)
                : GetOrCreateMeshApi(httpClient, jsonSerializer);
        }

        public void Dispose()
        {
            _meshApis.Clear();
            _meshApis = null;
        }

        private IMeshApi GetDefaultMeshApi(HttpClient httpClient)
        {
            return _meshApis.TryGetValue(typeof(NewtonsoftJsonSerializer), out var meshApi)
                ? meshApi
                : GetOrCreateMeshApi(httpClient, new NewtonsoftJsonSerializer());
        }

        private IMeshApi GetOrCreateMeshApi(HttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            if (_meshApis.TryGetValue(jsonSerializer.GetType(), out var meshApi))
            {
                return meshApi;
            }

            meshApi = new MeshApiService(new HttpClientFacade(config =>
            {
                config.HttpClient = httpClient;
                config.JsonSerializer = jsonSerializer;
            }));

            _meshApis.Add(jsonSerializer.GetType(), meshApi);

            return meshApi;
        }
    }
}