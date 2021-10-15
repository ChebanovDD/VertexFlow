namespace VertexFlow.Core.Models
{
    public abstract class MeshData<TVector3>
    {
        public string Id { get; set; }
        public int[] Triangles { get; set; }
        public TVector3[] Vertices { get; set; }
        public TVector3[] Normals { get; set; }
    }
}