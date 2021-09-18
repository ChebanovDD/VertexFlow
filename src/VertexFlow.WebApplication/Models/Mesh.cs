using VertexFlow.Core.Structs;

namespace VertexFlow.WebApplication.Models
{
    public record Mesh(int Id, int[] Triangles, Vector3[] Vertices, Vector3[] Normals);
}