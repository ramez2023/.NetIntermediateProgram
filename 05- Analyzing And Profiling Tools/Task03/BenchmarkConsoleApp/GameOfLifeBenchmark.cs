using System.Windows.Controls;
using BenchmarkDotNet.Attributes;

namespace BenchmarkConsoleApp;

[MemoryDiagnoser]
public class GameOfLifeBenchmark
{

    [Benchmark]
    public void OriginalMethod() => new GameOfLife.OriginalGrid(new Canvas()).Update();

    [Benchmark]
    public void OptimizedMethod() => new GameOfLife.Grid(new Canvas()).Update();
}