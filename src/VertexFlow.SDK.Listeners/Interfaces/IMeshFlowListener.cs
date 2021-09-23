using System;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Listeners.Interfaces
{
    public interface IMeshFlowListener : IDisposable
    {
        event EventHandler<string> MeshUpdated;
        
        Task<IMeshFlowListener> StartAsync();
        Task<IMeshFlowListener> StopAsync();
    }
}