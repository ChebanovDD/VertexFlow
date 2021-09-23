using VertexFlow.SDK.Extensions.Interfaces;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Extensions.Extensions
{
    public static class MeshFlowListenerExtensions
    {
        public static IMeshFlowListenerDecorator<TMeshData> WithStore<TMeshData>(this IMeshFlowListener listener,
            IMeshStore<TMeshData> store)
        {
            return new MeshFlowListenerDecorator<TMeshData>(listener, store);
        }
    }
}