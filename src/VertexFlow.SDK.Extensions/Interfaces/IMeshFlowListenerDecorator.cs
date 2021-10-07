using System;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Extensions.Interfaces
{
    /// <summary>
    /// Extension for <see cref="IMeshFlowListener"/>.
    /// </summary>
    /// <typeparam name="TMeshData">The type of object representing the mesh data.</typeparam>
    public interface IMeshFlowListenerDecorator<out TMeshData> : IMeshFlowListener
    {
        /// <summary>
        /// Allows to set an action to monitor mesh data adding to the database.
        /// </summary>
        /// <param name="action"><see cref="Action{TMeshData}"/> will be invoked when new mesh data is added to the database.</param>
        /// <returns><see cref="IMeshFlowListenerDecorator{TMeshData}"/> interface.</returns>
        IMeshFlowListenerDecorator<TMeshData> OnMeshCreated(Action<TMeshData> action);
        
        /// <summary>
        /// Allows to set an action to monitor mesh data updating to the database.
        /// </summary>
        /// <param name="action"><see cref="Action{TMeshData}"/> will be invoked when new mesh data is updated in the database.</param>
        /// <returns><see cref="IMeshFlowListenerDecorator{TMeshData}"/> interface.</returns>
        IMeshFlowListenerDecorator<TMeshData> OnMeshUpdated(Action<TMeshData> action);
        
        /// <summary>
        /// Configures an awaiter used to await <see cref="OnMeshCreated"/> and <see cref="OnMeshUpdated"/>.
        /// </summary>
        /// <param name="value"><see langword="true"/> to attempt to marshal the continuation back to the original context captured; otherwise, <see langword="false"/>.</param>
        /// <returns><see cref="IMeshFlowListenerDecorator{TMeshData}"/> interface.</returns>
        IMeshFlowListenerDecorator<TMeshData> ContinueOnCapturedContext(bool value = true);
    }
}