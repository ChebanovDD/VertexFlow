using Mapster;
using VertexFlow.Contracts.Protos;
using VertexFlow.SDK.Internal.Interfaces;

namespace VertexFlow.SDK.Internal
{
    internal class MeshMapper : IMeshMapper
    {
        public MeshMapper()
        {
            TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);
        }
        
        public MeshDataProto From<T>(T meshData)
        {
            return meshData.Adapt<MeshDataProto>();
        }

        public T To<T>(MeshDataProto meshDataProto)
        {
            return meshDataProto.Adapt<T>();
        }
    }
}