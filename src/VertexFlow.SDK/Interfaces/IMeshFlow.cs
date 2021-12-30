using System.Threading;
using System.Threading.Tasks;
using VertexFlow.Core.Interfaces;

namespace VertexFlow.SDK.Interfaces
{
    /// <summary>
    /// Defines methods for adding or replacing existing meshes in a database.
    /// </summary>
    /// <typeparam name="TMeshData">The type of object representing the mesh data.</typeparam>
    public interface IMeshFlow<in TMeshData> : IProjectContainer where TMeshData : IMeshData
    {
        /// <summary>
        /// Adds a mesh as an asynchronous operation to the database.
        /// </summary>
        /// <param name="mesh">A JSON serializable object that must contain mesh data.</param>
        /// <param name="cancellationToken">(Optional) <see cref="T:System.Threading.CancellationToken"/> representing request cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous send.</returns>
        /// <remarks>Will fail if there already is a mesh data with the same id.</remarks>
        Task SendAsync(TMeshData mesh, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Updates a mesh as an asynchronous operation in the database.
        /// </summary>
        /// <param name="mesh">A JSON serializable object that must contain mesh data.</param>
        /// <param name="cancellationToken">(Optional) <see cref="T:System.Threading.CancellationToken"/> representing request cancellation.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous update.</returns>
        /// <remarks>Will add or replace any mesh data with the specified id.</remarks>
        Task UpdateAsync(TMeshData mesh, CancellationToken cancellationToken = default);
    }
}