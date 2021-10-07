using System;
using System.Threading;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Listeners.Interfaces
{
    /// <summary>
    /// A mesh data change listener.
    /// </summary>
    /// <remarks>
    /// Establish a connection using <see cref="StartAsync"/>.
    /// Clean up a connection using <see cref="StopAsync"/>.
    /// </remarks>
    public interface IMeshFlowListener : IDisposable
    {
        /// <summary>
        /// Occurs when new mesh data has been added to the database.
        /// </summary>
        /// <remarks>
        /// The <see cref="string"/> parameter will contain the id of the created mesh.
        /// </remarks>
        event EventHandler<string> MeshCreated;

        /// <summary>
        /// Occurs when new mesh data has been updated in the database.
        /// </summary>
        /// <remarks>
        /// The <see cref="string"/> parameter will contain the id of the updated mesh.
        /// </remarks>
        event EventHandler<string> MeshUpdated;

        /// <summary>
        /// Starts listening to the server.
        /// </summary>
        /// <param name="onException"><see cref="Action{T}"/> will be invoked if an error occurred during the connection.</param>
        /// <param name="cancellationToken">(Optional) The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> containing <see cref="IMeshFlowListener"/> interface.</returns>
        Task<IMeshFlowListener> StartAsync(Action<Exception> onException = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Stops listening to the server.
        /// </summary>
        /// <param name="cancellationToken">(Optional) The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task"/> containing <see cref="IMeshFlowListener"/> interface.</returns>
        Task<IMeshFlowListener> StopAsync(CancellationToken cancellationToken = default);
    }
}