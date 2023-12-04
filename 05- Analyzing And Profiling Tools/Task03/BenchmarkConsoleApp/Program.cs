using BenchmarkDotNet.Running;

namespace BenchmarkConsoleApp;

internal class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<GameOfLifeBenchmark>();
    }
}