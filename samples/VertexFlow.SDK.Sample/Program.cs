using System;
using System.Net.Http;
using System.Threading.Tasks;
using VertexFlow.SDK.Extensions;
using VertexFlow.SDK.Listeners;

namespace VertexFlow.SDK.Sample
{
    static class Program
    {
        static async Task Main()
        {
            using var vertexFlow = new VertexFlow("https://localhost:5001");
            
            var meshFlow = vertexFlow.CreateMeshFlow<CustomMesh>();
            var meshStore = vertexFlow.CreateMeshStore<CustomMesh>();
            
            using var meshFlowListener = await vertexFlow
                .CreateMeshFlowListener()
                .WithStore(meshStore)
                .OnMeshUpdated(mesh => Console.WriteLine($"Mesh '{mesh.Id}' updated."))
                .StartAsync(exception => throw new HttpRequestException(exception.Message));
            
            foreach (var mesh in await meshStore.GetAllAsync())
            {
                Console.WriteLine($"Mesh '{mesh.Id}' downloaded.");
                
                await meshFlow.UpdateAsync(mesh.Id, mesh);
                await Task.Delay(100);
            }
            
            await meshFlowListener.StopAsync();
        }
    }
}