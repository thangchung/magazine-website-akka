using BenchmarkDotNet.Running;

namespace Cik.Magazine.Benchmark.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<CategoryServiceBenchmark>();
            System.Console.WriteLine(summary);
            System.Console.ReadKey();
        }
    }
}