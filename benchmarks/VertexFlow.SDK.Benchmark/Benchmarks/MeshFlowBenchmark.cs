using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using VertexFlow.SDK.Benchmark.JsonSerializers;
using VertexFlow.SDK.Benchmark.Models;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Benchmark.Benchmarks
{
    [MemoryDiagnoser, Orderer(SummaryOrderPolicy.Declared)]
    public class MeshFlowBenchmark
    {
        private const string Server = "https://localhost:5001";

        private IEnumerable<CustomMesh> _customMeshes;
        
        private VertexFlow _vertexFlow;

        private IMeshFlow<CustomMesh> _newtonsoftMeshFlow;
        private IMeshFlow<CustomMesh> _systemTextStreamMeshFlow;
        private IMeshFlow<CustomMesh> _systemTextRecyclableStreamMeshFlow;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            _vertexFlow = new VertexFlow(Server);
            
            _newtonsoftMeshFlow = _vertexFlow
                .CreateMeshFlow<CustomMesh>();
            
            _systemTextStreamMeshFlow = _vertexFlow
                .CreateMeshFlow<CustomMesh>(new SystemTextSerializerMemoryStream());
            
            _systemTextRecyclableStreamMeshFlow = _vertexFlow
                .CreateMeshFlow<CustomMesh>(new SystemTextSerializerRecyclableMemoryStream());

            _customMeshes = await _vertexFlow.CreateMeshStore<CustomMesh>().GetAllAsync();
        }

        [Benchmark(Baseline = true)]
        public async Task SendAllMeshes_Newtonsoft_Stream()
        {
            var tasks = _customMeshes
                .Select(customMesh => _newtonsoftMeshFlow.UpdateAsync(customMesh.Id, customMesh));

            await Task.WhenAll(tasks);
        }
        
        [Benchmark]
        public async Task SendAllMeshes_SystemTextJson_Stream()
        {
            var tasks = _customMeshes
                .Select(customMesh => _systemTextStreamMeshFlow.UpdateAsync(customMesh.Id, customMesh));

            await Task.WhenAll(tasks);
        }

        [Benchmark]
        public async Task SendAllMeshes_SystemTextJson_RecyclableStream()
        {
            var tasks = _customMeshes
                .Select(customMesh => _systemTextRecyclableStreamMeshFlow.UpdateAsync(customMesh.Id, customMesh));

            await Task.WhenAll(tasks);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _customMeshes = null;
            _vertexFlow?.Dispose();
        }
    }
}