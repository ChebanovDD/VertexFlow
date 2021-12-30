using ProtoBuf;

namespace VertexFlow.Contracts.Protos
{
    [ProtoContract]
    public struct Vector3Proto
    {
        [ProtoMember(1)]
        public float X { get; set; }
        
        [ProtoMember(2)]
        public float Y { get; set; }
        
        [ProtoMember(3)]
        public float Z { get; set; }
    }
}