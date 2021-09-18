using VertexFlow.Core.Structs;

namespace VertexFlow.WebAPI.Contracts
{
    public abstract class MeshData
    {
        public int Id { get; set; }
        public int[] Triangles { get; set; }
        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }
    }
}