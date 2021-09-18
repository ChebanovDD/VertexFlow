using VertexFlow.Core.Structs;

namespace VertexFlow.WebApplication.Models
{
    public record Mesh(string Id, int[] Triangles, Vector3[] Vertices, Vector3[] Normals);
}