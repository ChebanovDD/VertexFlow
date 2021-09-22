using System;
using System.Threading.Tasks;
using VertexFlow.SDK.Listeners;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Sample
{
    static class Program
    {
        private static IMeshStore<CustomMesh> _meshStore;

        static async Task Main()
        {
            using var vertexFlow = new VertexFlow("https://localhost:5001");

            await using var meshFlowListener = vertexFlow.CreateMeshFlowListener();
            meshFlowListener.MeshUpdated += OnMeshUpdated;
            await meshFlowListener.StartAsync();
            
            _meshStore = vertexFlow.CreateMeshStore<CustomMesh>();
            var meshFlow = vertexFlow.CreateMeshFlow<CustomMesh>();
            
            await foreach (var mesh in _meshStore.GetAllAsync())
            {
                Console.WriteLine(mesh.Id);

                await Task.Delay(1000);
                await meshFlow.UpdateAsync(mesh.Id, mesh);
            }
            
            meshFlowListener.MeshUpdated -= OnMeshUpdated;
        }

        private static async void OnMeshUpdated(object sender, string meshId)
        {
            var mesh = await _meshStore.GetAsync(meshId);
            if (mesh != null)
            {
                Console.WriteLine($"Mesh '{mesh.Id}' updated.");
            }
        }
    }
}