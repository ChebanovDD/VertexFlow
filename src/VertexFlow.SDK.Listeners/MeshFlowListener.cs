using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Listeners
{
    internal class MeshFlowListener : IMeshFlowListener
    {
        private readonly HubConnection _hubConnection;
        private readonly List<IDisposable> _disposables = new List<IDisposable>();

        public event EventHandler<MeshEventArgs> MeshCreated;
        public event EventHandler<MeshEventArgs> MeshUpdated;

        public MeshFlowListener(string server)
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{server}/notification")
                .Build();

            var meshCreatedConnection = _hubConnection.On<string, string>("Created", OnMeshCreated);
            var meshUpdatedConnection = _hubConnection.On<string, string>("Updated", OnMeshUpdated);

            _disposables.Add(meshCreatedConnection);
            _disposables.Add(meshUpdatedConnection);
        }

        public async Task<IMeshFlowListener> StartAsync(Action<Exception> onException = null,
            CancellationToken cancellationToken = default)
        {
            if (_hubConnection.State != HubConnectionState.Disconnected)
            {
                return this;
            }

            try
            {
                await _hubConnection.StartAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception) when (onException != null)
            {
                onException(exception);
            }

            return this;
        }

        public async Task<IMeshFlowListener> StopAsync(CancellationToken cancellationToken = default)
        {
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.StopAsync(cancellationToken).ConfigureAwait(false);
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

        private void OnMeshCreated(string projectName, string meshId)
        {
            MeshCreated?.Invoke(this, new MeshEventArgs(projectName, meshId));
        }

        private void OnMeshUpdated(string projectName, string meshId)
        {
            MeshUpdated?.Invoke(this, new MeshEventArgs(projectName, meshId));
        }
    }
}