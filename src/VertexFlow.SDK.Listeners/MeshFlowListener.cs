using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Listeners
{
    internal class MeshFlowListener : IMeshFlowListener
    {
        private readonly HubConnection _hubConnection;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        
        public event EventHandler<string> MeshUpdated;

        public MeshFlowListener(string server)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{server}/notification")
                .Build();

            var update = _hubConnection.On<string>("Update", OnMeshUpdated);
            _disposables.Add(update);
        }

        public async Task<IMeshFlowListener> StartAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync().ConfigureAwait(false);
            }

            return this;
        }

        public async Task<IMeshFlowListener> StopAsync()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync().ConfigureAwait(false);
            }

            return this;
        }

        public void Dispose()
        {
            for (var i = 0; i < _disposables.Count; i++)
            {
                _disposables[i].Dispose();
            }
        }
        
        private void OnMeshUpdated(string meshId)
        {
            MeshUpdated?.Invoke(this, meshId);
        }
    }
}