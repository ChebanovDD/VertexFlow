namespace VertexFlow.Contracts.Models
{
    public abstract record MeshData<TVector3>
    {
        public string Id { get; init; }
        public int[] Triangles { get; init; }
        public TVector3[] Vertices { get; init; }
        public TVector3[] Normals { get; init; }
    }
}