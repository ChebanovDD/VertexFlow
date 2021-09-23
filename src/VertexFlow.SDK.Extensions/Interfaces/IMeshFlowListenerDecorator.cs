using System;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Extensions.Interfaces
{
    public interface IMeshFlowListenerDecorator<out TMeshData> : IMeshFlowListener
    {
        IMeshFlowListener OnMeshUpdated(Action<TMeshData> action);
    }
}