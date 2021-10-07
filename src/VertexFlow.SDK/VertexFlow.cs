using System;
using System.Net.Http;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK
{
    public class VertexFlow : IDisposable
    {
        private bool _isDisposed;
        private readonly HttpClient _httpClient;
        private readonly MeshApiProvider _meshApiProvider;
        
        public string Server => _httpClient?.BaseAddress.OriginalString;
        
        public VertexFlow(string server, string version = "1.0")
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(server) };
            _httpClient.DefaultRequestHeaders.Add("version", version);
            _meshApiProvider = new MeshApiProvider();
        }
        
        /// <summary>
        /// Returns an implementation of the <see cref="IMeshFlow{TMeshData}"/> interface.
        /// Which defines methods for adding or replacing existing meshes in a database.
        /// </summary>
        /// <param name="jsonSerializer">(Optional) Enables you to control how objects are encoded into JSON.</param>
        /// <typeparam name="TMeshData">The type of object representing the mesh data.</typeparam>
        /// <returns><see cref="IMeshFlow{TMeshData}"/> interface.</returns>
        public IMeshFlow<TMeshData> CreateMeshFlow<TMeshData>(IJsonSerializer jsonSerializer = null)
        {
            return new MeshFlow<TMeshData>(GetMeshApi(jsonSerializer));
        }
        
        /// <summary>
        /// Returns an implementation of the <see cref="IMeshStore{TMeshData}"/> interface.
        /// Which defines methods for reading or deleting existing meshes from a database.
        /// </summary>
        /// <param name="jsonSerializer">(Optional) Enables you to control how objects are encoded into JSON.</param>
        /// <typeparam name="TMeshData">The type of object representing the mesh data.</typeparam>
        /// <returns><see cref="IMeshStore{TMeshData}"/> interface.</returns>
        public IMeshStore<TMeshData> CreateMeshStore<TMeshData>(IJsonSerializer jsonSerializer = null)
        {
            return new MeshStore<TMeshData>(GetMeshApi(jsonSerializer));
        }
        
        /// <summary>
        /// Releases and disposes of resources used by the <see cref="T:System.Net.Http.HttpClient"/>.
        /// </summary>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            _httpClient?.Dispose();
            _meshApiProvider?.Dispose();
        }
        
        private IMeshApi GetMeshApi(IJsonSerializer jsonSerializer)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(VertexFlow));
            }
            
            return _meshApiProvider.GetMeshApi(_httpClient, jsonSerializer);
        }
    }
}