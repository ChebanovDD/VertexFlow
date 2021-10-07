using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    /// <summary>
    /// Defines methods to convert between an object and <see cref="HttpContent"/> that includes JSON.
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Convert an object to <see cref="HttpContent"/> that includes JSON.
        /// </summary>
        /// <param name="data">The object to serialize.</param>
        /// <param name="cancellationToken">The <see cref="System.Threading.CancellationToken"/> which may be used to cancel the write operation.</param>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> containing a <see cref="HttpContent"/> representation of the object.</returns>
        Task<HttpContent> SerializeAsync<T>(T data, CancellationToken cancellationToken);
        
        /// <summary>
        /// Convert <see cref="HttpContent"/> that includes JSON to an object.
        /// </summary>
        /// <param name="httpContent"><see cref="HttpContent"/> that includes JSON.</param>
        /// <param name="cancellationToken">The <see cref="System.Threading.CancellationToken"/> which may be used to cancel the read operation.</param>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> containing the instance of <typeparamref name="T"/> being deserialized.</returns>
        Task<T> DeserializeAsync<T>(HttpContent httpContent, CancellationToken cancellationToken);
    }
}