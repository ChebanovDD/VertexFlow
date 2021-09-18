using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VertexFlow.WebInfrastructure
{
    public interface IMeshHub
    {
        Task Update(string meshId);
    }
    
    public class MeshHub : Hub<IMeshHub>
    {
    }
}