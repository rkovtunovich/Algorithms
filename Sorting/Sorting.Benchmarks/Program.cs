
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Sorting.Benchmarks;

var config = new ManualConfig();
config.Add(DefaultConfig.Instance);
config.ArtifactsPath = "C:\\repos\\learning\\Algo\\Sorting\\Sorting.Benchmarks\\Reports\\";

//BenchmarkRunner.Run<MergeInsertionSortBenchmark>(config);
//BenchmarkRunner.Run<InsertionSortBenchmark>(config);
BenchmarkRunner.Run<MergeSortBenchmark>(config);
