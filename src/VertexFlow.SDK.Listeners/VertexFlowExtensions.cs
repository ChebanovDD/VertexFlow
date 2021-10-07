using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Listeners
{
    public static class VertexFlowExtensions
    {
        /// <summary>
        /// Returns an implementation of the <see cref="IMeshFlowListener"/> interface.
        /// Which provides methods to monitor mesh data changes.
        /// </summary>
        /// <param name="vertexFlow"><see cref="VertexFlow"/></param>
        /// <returns><see cref="IMeshFlowListener"/> interface.</returns>
        public static IMeshFlowListener CreateMeshFlowListener(this VertexFlow vertexFlow)
        {
            return new MeshFlowListener(vertexFlow.Server);
        }
    }
}