using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VertexFlow.WebApplication.Interfaces;

namespace VertexFlow.WebInfrastructure
{
    internal class MeshNotifier : IMeshNotifier
    {
        private readonly IHubContext<MeshHub, IMeshHub> _meshNotificationHub;

        public MeshNotifier(IHubContext<MeshHub, IMeshHub> meshNotificationHub)
        {
            _meshNotificationHub = meshNotificationHub;
        }

        public async Task Created(string meshId)
        {
            await _meshNotificationHub.Clients.All.Created(meshId).ConfigureAwait(false);
        }
        
        public async Task Updated(string meshId)
        {
            await _meshNotificationHub.Clients.All.Updated(meshId).ConfigureAwait(false);
        }
    }
}