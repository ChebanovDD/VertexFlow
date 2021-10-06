using BenchmarkDotNet.Running;
using VertexFlow.SDK.Benchmark.Benchmarks;

namespace VertexFlow.SDK.Benchmark
{
    static class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<MeshFlowBenchmark>();
            BenchmarkRunner.Run<MeshStoreBenchmark>();
        }
    }
}