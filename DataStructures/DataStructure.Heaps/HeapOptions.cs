namespace DataStructures.Heaps;

public class HeapOptions<TKey> 
{
    public int Capacity { get; set; } = 16;

    public int Degree { get; set; } = 2;

    public bool UseKeyTracking { get; set; } = false;

    public bool UseValueTracking { get; set; } = false;

    public IComparer<TKey>? Comparer { get; set; }
}
