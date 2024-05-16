using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

var config = new ManualConfig();
config.Add(DefaultConfig.Instance);
config.ArtifactsPath = "C:\\repos\\learning\\Algo\\Searching\\Searching.Benchmarks\\Reports\\";

BenchmarkRunner.Run<MaxSumSubArrayBenchmark>(config);
