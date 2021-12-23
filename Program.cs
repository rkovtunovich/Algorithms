
using Helpers;
using Sorting;
using System;
using System.Diagnostics;

var array = ArrayHelper.GetUnsortedArray();

var stopWatch = new Stopwatch();

stopWatch.Start();
MergeSort.Sort(ref array);
stopWatch.Stop();

Console.WriteLine($"-------------------");
Console.WriteLine($"Run time {stopWatch.Elapsed}");

stopWatch.Start();
MergeSort2.Sort(ref array);
stopWatch.Stop();

ArrayHelper.ShowArray(array);

Console.WriteLine($"-------------------");
Console.WriteLine($"Run time {stopWatch.Elapsed}");

array = ArrayHelper.GetUnsortedArray(5);

ArrayHelper.ShowArray(array);

Console.WriteLine($"array contains {ArrayInversions.Count(ref array)} inversions"); 
Console.ReadKey();