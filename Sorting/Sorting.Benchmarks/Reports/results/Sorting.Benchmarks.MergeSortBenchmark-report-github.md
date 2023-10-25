```

BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 11 (10.0.22621.2428/22H2/2022Update/SunValley2)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2 [AttachedDebugger]
  Job-FIPCDQ : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
| Method | N      | Mean        | Error      | StdDev     | Median      | Gen0      | Allocated   |
|------- |------- |------------:|-----------:|-----------:|------------:|----------:|------------:|
| **Sort**   | **100**    |    **16.83 μs** |   **0.442 μs** |   **1.281 μs** |    **16.70 μs** |         **-** |     **8.62 KB** |
| **Sort**   | **1000**   |   **168.37 μs** |   **3.330 μs** |   **6.005 μs** |   **166.00 μs** |         **-** |    **94.46 KB** |
| **Sort**   | **10000**  | **1,694.58 μs** |  **32.987 μs** |  **54.198 μs** | **1,705.70 μs** |         **-** |  **1086.37 KB** |
| **Sort**   | **100000** | **7,406.78 μs** | **146.067 μs** | **397.385 μs** | **7,251.10 μs** | **1000.0000** | **12140.24 KB** |
