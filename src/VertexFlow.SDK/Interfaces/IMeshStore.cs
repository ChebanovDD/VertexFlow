using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Interfaces
{
    /// <summary>
    /// Defines methods for reading or deleting existing meshes from a database.
    /// </summary>
    /// <typeparam name="TMeshData">The type of object representing the mesh data.</typeparam>
    public interface IMeshStore<TMeshData> : IProjectContainer
    {
        /// <summary>
        /// Reads a mesh data from the database as an asynchronous operation.
        /// </summary>
        /// <param name="meshId">The mesh id.</param>
        /// <param name="cancellationToken">(Optional) <see cref="T:System.Threading.CancellationToken"/> representing request cancellation.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> containing a mesh data.</returns>
        Task<TMeshData> GetAsync(string meshId, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Reads all meshes from the database as an asynchronous operation.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> containing an iterator to go through the meshes.</returns>
        Task<IEnumerable<TMeshData>> GetAllAsync(); // TODO: Change to IAsyncEnumerable with ASP.NET Core 6.0
        
        /// <summary>
        /// Delete a mesh data from the database as an asynchronous operation.
        /// </summary>
        /// <param name="meshId">The mesh id.</param>
        /// <param name="cancellationToken">(Optional) <see cref="T:System.Threading.CancellationToken"/> representing request cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous delete.</returns>
        Task DeleteAsync(string meshId, CancellationToken cancellationToken = default);
    }
}