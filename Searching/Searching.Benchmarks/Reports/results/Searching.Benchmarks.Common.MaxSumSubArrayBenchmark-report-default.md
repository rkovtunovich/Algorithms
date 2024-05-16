
BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 11 (10.0.22631.3527)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.300
  [Host]     : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2 [AttachedDebugger]
  DefaultJob : .NET 8.0.5 (8.0.524.21615), X64 RyuJIT AVX2


 Method         | N   | Mean        | Error     | StdDev    | Allocated |
--------------- |---- |------------:|----------:|----------:|----------:|
 **Find**           | **10**  |   **109.37 ns** |  **2.183 ns** |  **3.060 ns** |         **-** |
 FindBruteForce | 10  |    41.50 ns |  0.569 ns |  0.505 ns |         - |
 FindHybrid     | 10  |    44.05 ns |  0.709 ns |  0.663 ns |         - |
 **Find**           | **41**  |   **489.14 ns** |  **5.041 ns** |  **4.469 ns** |         **-** |
 FindBruteForce | 41  |   456.59 ns |  4.405 ns |  4.121 ns |         - |
 FindHybrid     | 41  |   277.93 ns |  5.490 ns |  8.708 ns |         - |
 **Find**           | **100** | **1,326.66 ns** | **26.107 ns** | **41.408 ns** |         **-** |
 FindBruteForce | 100 | 2,580.92 ns | 35.936 ns | 31.856 ns |         - |
 FindHybrid     | 100 |   769.53 ns |  9.050 ns |  8.023 ns |         - |
