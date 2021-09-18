using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VertexFlow.WebInfrastructure
{
    internal interface IMeshHub
    {
        Task Update(string meshId);
    }
    
    internal class MeshHub : Hub<IMeshHub>
    {
    }
}