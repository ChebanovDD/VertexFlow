using System;
using System.Threading;
using System.Threading.Tasks;
using VertexFlow.SDK.Extensions.Extensions;
using VertexFlow.SDK.Extensions.Interfaces;
using VertexFlow.SDK.Interfaces;
using VertexFlow.SDK.Listeners.Interfaces;

namespace VertexFlow.SDK.Extensions
{
    internal class MeshFlowListenerDecorator<TMeshData> : IMeshFlowListenerDecorator<TMeshData>
    {
        private bool _continueOnCapturedContext = true;
        private Action<TMeshData> _onMeshCreated;
        private Action<TMeshData> _onMeshUpdated;
        private readonly IMeshStore<TMeshData> _meshStore;
        private readonly IMeshFlowListener _meshFlowListener;

        public event EventHandler<string> MeshCreated
        {
            add => _meshFlowListener.MeshCreated += value;
            remove => _meshFlowListener.MeshCreated -= value;
        }

        public event EventHandler<string> MeshUpdated
        {
            add => _meshFlowListener.MeshUpdated += value;
            remove => _meshFlowListener.MeshUpdated -= value;
        }

        public MeshFlowListenerDecorator(IMeshFlowListener meshFlowListener, IMeshStore<TMeshData> meshStore)
        {
            _meshStore = meshStore;
            _meshFlowListener = meshFlowListener;
            _meshFlowListener.MeshCreated += OnMeshCreated;
            _meshFlowListener.MeshUpdated += OnMeshUpdated;
        }

        public async Task<IMeshFlowListener> StartAsync(Action<Exception> onException = null,
            CancellationToken cancellationToken = default)
        {
            return await _meshFlowListener.StartAsync(onException, cancellationToken).ConfigureAwait(false);
        }

        public IMeshFlowListenerDecorator<TMeshData> OnMeshCreated(Action<TMeshData> action)
        {
            _onMeshCreated = action;
            return this;
        }

        public IMeshFlowListenerDecorator<TMeshData> OnMeshUpdated(Action<TMeshData> action)
        {
            _onMeshUpdated = action;
            return this;
        }

        public IMeshFlowListenerDecorator<TMeshData> ContinueOnCapturedContext(bool value = true)
        {
            _continueOnCapturedContext = value;
            return this;
        }

        public async Task<IMeshFlowListener> StopAsync(CancellationToken cancellationToken = default)
        {
            return await _meshFlowListener.StopAsync(cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            _onMeshCreated = null;
            _onMeshUpdated = null;
            _meshFlowListener.MeshCreated -= OnMeshCreated;
            _meshFlowListener.MeshUpdated -= OnMeshUpdated;
            _meshFlowListener.Dispose();
        }

        private void OnMeshCreated(object sender, string meshId)
        {
            RaiseMeshChanged(meshId, _onMeshCreated, _continueOnCapturedContext).Forget(false);
        }

        private void OnMeshUpdated(object sender, string meshId)
        {
            RaiseMeshChanged(meshId, _onMeshUpdated, _continueOnCapturedContext).Forget(false);
        }

        private async Task RaiseMeshChanged(string meshId, Action<TMeshData> action, bool configureAwait)
        {
            if (action == null)
            {
                return;
            }

            var mesh = await _meshStore.GetAsync(meshId).ConfigureAwait(configureAwait);
            if (mesh != null)
            {
                action(mesh);
            }
        }
    }
}