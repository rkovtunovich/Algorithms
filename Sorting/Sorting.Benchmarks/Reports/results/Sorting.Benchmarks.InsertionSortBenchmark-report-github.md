```

BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 11 (10.0.22621.2428/22H2/2022Update/SunValley2)
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100-rc.2.23502.2
  [Host]     : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2
  Job-KBUDOF : .NET 8.0.0 (8.0.23.47906), X64 RyuJIT AVX2

InvocationCount=1  UnrollFactor=1  

```
| Method     | N      | Mean            | Error         | StdDev        | Allocated |
|----------- |------- |----------------:|--------------:|--------------:|----------:|
| **Sort_Array** | **100**    |        **12.43 μs** |      **0.227 μs** |      **0.615 μs** |     **400 B** |
| Sort_Span  | 100    |        12.10 μs |      0.415 μs |      1.142 μs |     400 B |
| **Sort_Array** | **1000**   |       **373.68 μs** |      **4.786 μs** |      **3.997 μs** |     **400 B** |
| Sort_Span  | 1000   |       199.53 μs |      3.932 μs |      4.037 μs |     400 B |
| **Sort_Array** | **10000**  |    **37,940.26 μs** |    **535.243 μs** |    **474.479 μs** |     **400 B** |
| Sort_Span  | 10000  |    19,303.90 μs |    294.114 μs |    275.115 μs |     400 B |
| **Sort_Array** | **100000** | **3,774,591.38 μs** | **24,413.768 μs** | **22,836.654 μs** |     **400 B** |
| Sort_Span  | 100000 | 1,891,512.30 μs | 14,926.906 μs | 13,962.637 μs |     400 B |
