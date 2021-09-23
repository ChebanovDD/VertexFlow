using System;
using System.Threading.Tasks;
using VertexFlow.SDK.Extensions.Interfaces;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Extensions
{
    internal class MeshFlowListenerDecorator<TMeshData> : IMeshFlowListenerDecorator<TMeshData>
    {
        private Action<TMeshData> _onMeshUpdated;
        private readonly IMeshStore<TMeshData> _meshStore;
        private readonly IMeshFlowListener _meshFlowListener;

        public event EventHandler<string> MeshUpdated
        {
            add => _meshFlowListener.MeshUpdated += value;
            remove => _meshFlowListener.MeshUpdated -= value;
        }

        public MeshFlowListenerDecorator(IMeshFlowListener meshFlowListener, IMeshStore<TMeshData> meshStore)
        {
            _meshStore = meshStore;
            _meshFlowListener = meshFlowListener;
            _meshFlowListener.MeshUpdated += OnMeshUpdated;
        }

        public async Task<IMeshFlowListener> StartAsync()
        {
            return await _meshFlowListener.StartAsync().ConfigureAwait(false);
        }

        public IMeshFlowListener OnMeshUpdated(Action<TMeshData> action)
        {
            _onMeshUpdated = action;
            return this;
        }

        public async Task<IMeshFlowListener> StopAsync()
        {
            return await _meshFlowListener.StopAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _onMeshUpdated = null;
            _meshFlowListener.MeshUpdated -= OnMeshUpdated;
            _meshFlowListener.Dispose();
        }

        private async void OnMeshUpdated(object sender, string meshId)
        {
            if (_onMeshUpdated == null)
            {
                return;
            }
            
            try
            {
                var mesh = await _meshStore.GetAsync(meshId).ConfigureAwait(false);
                if (mesh != null)
                {
                    _onMeshUpdated(mesh);
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}