using System;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Listeners.Interfaces
{
    public interface IMeshFlowListener : IDisposable
    {
        event EventHandler<string> MeshCreated;
        event EventHandler<string> MeshUpdated;
        
        Task<IMeshFlowListener> StartAsync(Action<Exception> onException = null);
        Task<IMeshFlowListener> StopAsync();
    }
}