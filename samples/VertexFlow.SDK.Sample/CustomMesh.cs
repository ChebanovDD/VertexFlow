using VertexFlow.Core.Models;

namespace VertexFlow.SDK.Sample
{
    public struct CustomVector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class CustomMesh : MeshData<CustomVector3>
    {
    }
}