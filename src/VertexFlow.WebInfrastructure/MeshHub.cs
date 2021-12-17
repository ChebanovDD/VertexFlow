using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace VertexFlow.WebInfrastructure
{
    public interface IMeshHub
    {
        Task Created(string projectName, string meshId);
        Task Updated(string projectName, string meshId);
    }
    
    public class MeshHub : Hub<IMeshHub>
    {
    }
}