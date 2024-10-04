
namespace Searching.MediansAndOrderStatistics;

public struct WeightedValue(double value, double weight) : IComparable<WeightedValue>
{
    public double Value { get; set; } = value;

    public double Weight { get; set; } = weight;

    public int CompareTo(WeightedValue other)
    {
        return Value.CompareTo(other.Value);
    }
}