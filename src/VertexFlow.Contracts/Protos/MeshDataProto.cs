using ProtoBuf;

namespace VertexFlow.Contracts.Protos
{
    [ProtoContract]
    public class MeshDataProto
    {
        [ProtoMember(1)]
        public string Id { get; set; }
        
        [ProtoMember(2)]
        public int[] Triangles { get; set; }
        
        [ProtoMember(3)]
        public Vector3Proto[] Vertices { get; set; }
        
        [ProtoMember(4)]
        public Vector3Proto[] Normals { get; set; }
    }
}