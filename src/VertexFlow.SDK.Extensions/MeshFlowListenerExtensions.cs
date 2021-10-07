using VertexFlow.SDK.Extensions.Interfaces;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Extensions
{
    /// <summary>
    /// Provides additional methods that extend the <see cref="IMeshFlowListener"/> functionality.
    /// </summary>
    public static class MeshFlowListenerExtensions
    {
        /// <summary>
        /// Returns an implementation of the <see cref="IMeshFlowListenerDecorator{TMeshData}"/> interface.
        /// Which extends <see cref="IMeshFlowListener"/> interface.
        /// </summary>
        /// <param name="listener"><see cref="IMeshFlowListener"/></param>
        /// <param name="store"><see cref="IMeshStore{TMeshData}"/></param>
        /// <typeparam name="TMeshData">The type of object representing the mesh data.</typeparam>
        /// <returns><see cref="IMeshFlowListenerDecorator{TMeshData}"/> interface.</returns>
        public static IMeshFlowListenerDecorator<TMeshData> WithStore<TMeshData>(this IMeshFlowListener listener,
            IMeshStore<TMeshData> store)
        {
            return new MeshFlowListenerDecorator<TMeshData>(listener, store);
        }
    }
}