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
        private readonly List<IDisposable> _disposables = new();
        
        public event EventHandler<string> MeshUpdated;

        public MeshFlowListener(string server)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{server}/notification")
                .Build();

            var update = _hubConnection.On<string>("Update", OnMeshUpdated);
            _disposables.Add(update);
        }

        public async ValueTask DisposeAsync()
        {
            await StopConnectionAsync();

            for (var i = 0; i < _disposables.Count; i++)
            {
                _disposables[i].Dispose();
            }
        }

        public async Task StartAsync()
        {
            if (_hubConnection.State == HubConnectionState.Disconnected)
            {
                await _hubConnection.StartAsync();
            }
        }

        public async Task StopAsync()
        {
            await StopConnectionAsync();
        }

        private void OnMeshUpdated(string meshId)
        {
            MeshUpdated?.Invoke(this, meshId);
        }

        private async Task StopConnectionAsync()
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync();
            }
        }
    }
}