using System;
using System.Threading.Tasks;

namespace VertexFlow.SDK.Sample
{
    static class Program
    {
        static async Task Main()
        {
            using var vertexFlow = new VertexFlow("https://localhost:5001");
            
            var meshStore = vertexFlow.CreateMeshStore<CustomMesh>();
            await foreach (var mesh in meshStore.GetAllAsync())
            {
                Console.WriteLine(mesh.Id);
            }
        }
    }
}