using BenchmarkDotNet.Running;

namespace ORMByExample.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StronglyTypedIdVsGuidPerformance>();
        }
    }
}