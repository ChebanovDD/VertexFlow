using System;
using System.Net.Http;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Internal;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK
{
    /// <summary>
    /// Provides methods to create <see cref="IMeshFlow{TMeshData}"/> and <see cref="IMeshStore{TMeshData}"/>.
    /// </summary>
    public class VertexFlow : IDisposable
    {
        private bool _isDisposed;
        private readonly HttpClient _httpClient;
        private readonly MeshApiProvider _meshApiProvider;
        
        /// <summary>
        /// Gets the server base address (URI) used when sending requests.
        /// </summary>
        public string Server => _httpClient?.BaseAddress.OriginalString;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="VertexFlow"/>.
        /// </summary>
        /// <param name="server">Server base address.</param>
        /// <param name="version">(Optional) Server version. The default value is "1.0".</param>
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
        /// <param name="projectName">Project name.</param>
        /// <param name="jsonSerializer">(Optional) Enables you to control how objects are encoded into JSON.</param>
        /// <typeparam name="TMeshData">The type of object representing the mesh data.</typeparam>
        /// <returns><see cref="IMeshFlow{TMeshData}"/> interface.</returns>
        public IMeshFlow<TMeshData> CreateMeshFlow<TMeshData>(string projectName = null, IJsonSerializer jsonSerializer = null)
        {
            return new MeshFlow<TMeshData>(projectName, GetMeshApi(jsonSerializer));
        }

        /// <summary>
        /// Returns an implementation of the <see cref="IMeshStore{TMeshData}"/> interface.
        /// Which defines methods for reading or deleting existing meshes from a database.
        /// </summary>
        /// <param name="projectName">Project name.</param>
        /// <param name="jsonSerializer">(Optional) Enables you to control how objects are encoded into JSON.</param>
        /// <typeparam name="TMeshData">The type of object representing the mesh data.</typeparam>
        /// <returns><see cref="IMeshStore{TMeshData}"/> interface.</returns>
        public IMeshStore<TMeshData> CreateMeshStore<TMeshData>(string projectName = null, IJsonSerializer jsonSerializer = null)
        {
            return new MeshStore<TMeshData>(projectName, GetMeshApi(jsonSerializer));
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