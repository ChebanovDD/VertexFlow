using VertexFlow.Contracts.Protos;

namespace VertexFlow.SDK.Internal.Interfaces
{
    internal interface IMeshMapper
    {
        MeshDataProto From<T>(T meshData);
        T To<T>(MeshDataProto meshDataProto);
    }
}