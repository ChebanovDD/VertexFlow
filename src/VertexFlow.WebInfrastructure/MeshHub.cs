using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VertexFlow.WebInfrastructure
{
    public interface IMeshHub
    {
        Task Created(string meshId);
        Task Updated(string meshId);
    }
    
    public class MeshHub : Hub<IMeshHub>
    {
    }
}