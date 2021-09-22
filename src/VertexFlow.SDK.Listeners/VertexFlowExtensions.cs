using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Listeners
{
    public static class VertexFlowExtensions
    {
        public static IMeshFlowListener CreateMeshFlowListener(this VertexFlow vertexFlow)
        {
            return new MeshFlowListener(vertexFlow.Server);
        }
    }
}