using DataStructures.Heap;
using System.Numerics;

namespace MathAlgo;
public class Median<TKey, TValue> where TKey : INumber<TKey>
{
    private readonly HeapMin<TKey, TValue> _heapMin;
    private readonly HeapMax<TKey, TValue> _heapMax;

    public Median()
    {
        _heapMin = new();
        _heapMax = new();
    }

    public void Add(TKey value)
    {
        if (value <=_heapMax.Extremum.Key)
            _heapMax.Insert(value);
        else
            _heapMin.Insert(value);

        int lengthDiff = _heapMax.Length - _heapMin.Length;

        if (lengthDiff is 2)
        {
            _heapMin.Insert(_heapMax.ExtractKey());
        }
        else if(lengthDiff is -2)
        {
            _heapMax.Insert(_heapMin.ExtractKey());
        }
    }

    public TKey Current { get => _heapMax.Length >= _heapMin.Length ? _heapMax.Extremum.Key : _heapMin.Extremum.Key; }
}
