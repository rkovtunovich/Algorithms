using MathAlgo.Models;

namespace Scheduling.Models.Helpers;

public static class IntervalViewer
{
    public static void ShowIntervals<T>(List<T> intervals, int maxValue) where T : Interval
    {
        foreach (var interval in intervals)
        {
            Console.Write($"[{interval.Start} : {interval.End}] \t");
            Console.Write("[");

            if (interval.Start > interval.End)
            {
                for (var i = 0; i < interval.End; i++)
                    Console.Write("-");

                for (var i = interval.End; i < interval.Start; i++)
                    Console.Write(".");

                for (var i = interval.Start; i < maxValue; i++)
                    Console.Write("-");
            }
            else
            {
                for (var i = 0; i < interval.Start; i++)
                    Console.Write(".");

                for (var i = interval.Start; i < interval.End; i++)
                    Console.Write("-");

                for (var i = interval.End; i < maxValue; i++)
                    Console.Write(".");
            }

            Console.Write("] \n");
        }
    }
}
