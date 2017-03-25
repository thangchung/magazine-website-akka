using BenchmarkDotNet.Attributes;

namespace Cik.Magazine.Benchmark.Console
{
    // TODO: will add more code for benchmark the CategoryService later
    public class CategoryServiceBenchmark
    {
        private static int _counter = 1;

        [Benchmark]
        public int Increase()
        {
            return ++_counter;
        }
    }
}