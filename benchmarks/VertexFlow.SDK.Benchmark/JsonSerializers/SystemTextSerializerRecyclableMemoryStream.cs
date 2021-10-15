using System.IO;
using Microsoft.IO;

namespace VertexFlow.SDK.Benchmark.JsonSerializers
{
    public class SystemTextSerializerRecyclableMemoryStream : SystemTextSerializer
    {
        private readonly RecyclableMemoryStreamManager _memoryManager;

        public SystemTextSerializerRecyclableMemoryStream()
        {
            _memoryManager = new RecyclableMemoryStreamManager();
        }

        protected override MemoryStream GetMemoryStream()
        {
            return _memoryManager.GetStream();
        }
    }
}