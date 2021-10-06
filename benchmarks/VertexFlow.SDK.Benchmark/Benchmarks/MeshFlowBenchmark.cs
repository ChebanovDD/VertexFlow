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

        private CustomMesh[] _customMeshes;
        
        private VertexFlow _vertexFlow;

        private IMeshFlow<CustomMesh> _newtonsoftMeshFlow;
        private IMeshFlow<CustomMesh> _systemTextMeshFlow;
        private IMeshFlow<CustomMesh> _systemTextWithMemoryManagerMeshFlow;

        [GlobalSetup]
        public async Task GlobalSetup()
        {
            _vertexFlow = new VertexFlow(Server);
            
            _newtonsoftMeshFlow = _vertexFlow
                .CreateMeshFlow<CustomMesh>();
            
            _systemTextMeshFlow = _vertexFlow
                .CreateMeshFlow<CustomMesh>(new SystemTextSerializer());
            
            _systemTextWithMemoryManagerMeshFlow = _vertexFlow
                .CreateMeshFlow<CustomMesh>(new SystemTextSerializerWithMemoryManager());

            _customMeshes = await _vertexFlow.CreateMeshStore<CustomMesh>().GetAllAsync();
        }

        [Benchmark(Baseline = true)]
        public async Task SendAllMeshes_Newtonsoft_Stream()
        {
            var tasks = Enumerable.Range(0, _customMeshes.Length)
                .Select(i => _newtonsoftMeshFlow.UpdateAsync(_customMeshes[i].Id, _customMeshes[i]));

            await Task.WhenAll(tasks);
        }
        
        [Benchmark]
        public async Task SendAllMeshes_SystemTextJson_Stream()
        {
            var tasks = Enumerable.Range(0, _customMeshes.Length)
                .Select(i => _systemTextMeshFlow.UpdateAsync(_customMeshes[i].Id, _customMeshes[i]));

            await Task.WhenAll(tasks);
        }

        [Benchmark]
        public async Task SendAllMeshes_SystemTextJson_MemoryStreamManager()
        {
            var tasks = Enumerable.Range(0, _customMeshes.Length)
                .Select(i => _systemTextWithMemoryManagerMeshFlow.UpdateAsync(_customMeshes[i].Id, _customMeshes[i]));

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