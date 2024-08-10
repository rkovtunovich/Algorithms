using MathAlgo.Models;

namespace Scheduling.Models;

public record WeightedInterval : Interval
{
    public WeightedInterval(int start, int end, int weight) : base(start, end) => Weight = weight;

    public int Weight { get; init; }

    public override string ToString()
    {
        return $"[{Start},{End}] ({Weight})";
    }
}
