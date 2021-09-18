using System;
using System.Threading.Tasks;
using Refit;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Sample
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var meshesApi = RestService.For<IMeshesApi>("https://localhost:5001");

            var meshes = await meshesApi.GetAllAsync();
            if (meshes.Content == null)
            {
                return;
            }
            
            foreach (var mesh in meshes.Content)
            {
                Console.WriteLine(mesh.Id);
            }
        }
    }
}