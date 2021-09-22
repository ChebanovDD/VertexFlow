using System;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Listeners.Interfaces
{
    public interface IMeshFlowListener : IAsyncDisposable
    {
        event EventHandler<string> MeshUpdated;
        
        Task StartAsync();
        Task StopAsync();
    }
}