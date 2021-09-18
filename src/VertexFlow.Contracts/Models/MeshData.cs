using VertexFlow.Core.Structs;

namespace VertexFlow.Contracts.Models
{
    public abstract record MeshData
    {
        public string Id { get; init; }
        public int[] Triangles { get; init; }
        public Vector3[] Vertices { get; init; }
        public Vector3[] Normals { get; init; }
    }
}