using System;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Extensions.Interfaces
{
    public interface IMeshFlowListenerDecorator<out TMeshData> : IMeshFlowListener
    {
        IMeshFlowListenerDecorator<TMeshData> OnMeshCreated(Action<TMeshData> action);
        IMeshFlowListenerDecorator<TMeshData> OnMeshUpdated(Action<TMeshData> action);
        IMeshFlowListenerDecorator<TMeshData> ContinueOnCapturedContext(bool value = true);
    }
}