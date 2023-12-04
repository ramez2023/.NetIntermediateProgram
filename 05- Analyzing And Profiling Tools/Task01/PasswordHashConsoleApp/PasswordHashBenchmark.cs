using BenchmarkDotNet.Attributes;

namespace PasswordHashConsoleApp;

[MemoryDiagnoser]
public class PasswordHashBenchmark
{
    private readonly byte[] _salt = new byte[16];
    private const string PasswordText = "your_password_here";

    [Benchmark]
    public string OriginalMethod() =>
        PasswordHashGenerator.GeneratePasswordHashUsingSaltOriginal(PasswordText, _salt);

    [Benchmark]
    public string OptimizedMethod() =>
        PasswordHashGenerator.GeneratePasswordHashUsingSaltOptimized(PasswordText, _salt);
}