using System.IO;

namespace VertexFlow.SDK.Benchmark.JsonSerializers
{
    public class SystemTextSerializerMemoryStream : SystemTextSerializer
    {
        protected override MemoryStream GetMemoryStream()
        {
            return new MemoryStream();
        }
    }
}