using VertexFlow.Core.Interfaces;

namespace VertexFlow.Core.Models
{
    public abstract class MeshData<TVector3> : IMeshData
    {
        public string Id { get; set; }
        public int[] Triangles { get; set; }
        public TVector3[] Vertices { get; set; }
        public TVector3[] Normals { get; set; }
    }
}