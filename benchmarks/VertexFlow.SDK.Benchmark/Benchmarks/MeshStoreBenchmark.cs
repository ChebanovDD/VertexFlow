using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
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

        private Consumer _consumer;
        private VertexFlow _vertexFlow;

        private IMeshStore<CustomMesh> _newtonsoftMeshStore;
        private IMeshStore<CustomMesh> _systemTextStreamMeshStore;
        private IMeshStore<CustomMesh> _systemTextRecyclableStreamMeshStore;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _consumer = new Consumer();
            _vertexFlow = new VertexFlow(Server);

            _newtonsoftMeshStore = _vertexFlow
                .CreateMeshStore<CustomMesh>();

            _systemTextStreamMeshStore = _vertexFlow
                .CreateMeshStore<CustomMesh>(new SystemTextSerializerMemoryStream());

            _systemTextRecyclableStreamMeshStore = _vertexFlow
                .CreateMeshStore<CustomMesh>(new SystemTextSerializerRecyclableMemoryStream());
        }

        [Benchmark(Baseline = true)]
        public async Task GetAllMeshes_Newtonsoft_Stream()
        {
            (await _newtonsoftMeshStore.GetAllAsync()).Consume(_consumer);
        }
        
        [Benchmark]
        public async Task GetAllMeshes_SystemTextJson_Stream()
        {
            (await _systemTextStreamMeshStore.GetAllAsync()).Consume(_consumer);
        }
        
        [Benchmark]
        public async Task GetAllMeshes_SystemTextJson_RecyclableStream()
        {
            (await _systemTextRecyclableStreamMeshStore.GetAllAsync()).Consume(_consumer);
        }
        
        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _vertexFlow?.Dispose();
        }
    }
}