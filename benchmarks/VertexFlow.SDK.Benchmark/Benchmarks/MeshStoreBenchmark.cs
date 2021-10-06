using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using VertexFlow.SDK.Benchmark.JsonSerializers;
using VertexFlow.SDK.Benchmark.Models;
using VertexFlow.SDK.Interfaces;

namespace VertexFlow.SDK.Benchmark.Benchmarks
{
    [MemoryDiagnoser, Orderer(SummaryOrderPolicy.Declared)]
    public class MeshStoreBenchmark
    {
        private const string Server = "https://localhost:5001";

        private VertexFlow _vertexFlow;

        private IMeshStore<CustomMesh> _newtonsoftMeshStore;
        private IMeshStore<CustomMesh> _systemTextMeshStore;
        private IMeshStore<CustomMesh> _systemTextWithMemoryManagerMeshStore;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _vertexFlow = new VertexFlow(Server);

            _newtonsoftMeshStore = _vertexFlow
                .CreateMeshStore<CustomMesh>();

            _systemTextMeshStore = _vertexFlow
                .CreateMeshStore<CustomMesh>(new SystemTextSerializer());

            _systemTextWithMemoryManagerMeshStore = _vertexFlow
                .CreateMeshStore<CustomMesh>(new SystemTextSerializerWithMemoryManager());
        }

        [Benchmark(Baseline = true)]
        public async Task<CustomMesh[]> GetAllMeshes_Newtonsoft_Stream()
        {
            return await _newtonsoftMeshStore.GetAllAsync();
        }
        
        [Benchmark]
        public async Task<CustomMesh[]> GetAllMeshes_SystemTextJson_Stream()
        {
            return await _systemTextMeshStore.GetAllAsync();
        }
        
        [Benchmark]
        public async Task<CustomMesh[]> GetAllMeshes_SystemTextJson_MemoryStreamManager()
        {
            return await _systemTextWithMemoryManagerMeshStore.GetAllAsync();
        }
        
        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _vertexFlow?.Dispose();
        }
    }
}