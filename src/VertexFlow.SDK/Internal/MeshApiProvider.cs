using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal.Interfaces;
using VertexFlow.SDK.Internal.Serializers;
using VertexFlow.SDK.Internal.Services;

namespace VertexFlow.SDK.Internal
{
    internal class MeshApiProvider : IDisposable
    {
        private readonly IMeshMapper _meshMapper;
        private Dictionary<Type, IMeshApi> _meshApis;

        public MeshApiProvider()
        {
            _meshApis = new Dictionary<Type, IMeshApi>();
            _meshMapper = new MeshMapper();
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

            meshApi = new MeshApiService(CreateHttpClient(httpClient, jsonSerializer), _meshMapper);

            _meshApis.Add(jsonSerializer.GetType(), meshApi);

            return meshApi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IHttpClient CreateHttpClient(HttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            return new HttpClientFacade(httpClient, jsonSerializer);
        }
    }
}