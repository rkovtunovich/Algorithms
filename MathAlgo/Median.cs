using DataStructures;
using System.Numerics;

namespace MathAlgo;
public class Median<T> where T : INumber<T>
{
    private readonly HeapMin<T> _heapMin;
    private readonly HeapMax<T> _heapMax;

    public Median()
    {
        _heapMin = new();
        _heapMax = new();
    }

    public void Add(T value)
    {
        if (value <=_heapMax.Extremum)
            _heapMax.Insert(value);
        else
            _heapMin.Insert(value);

        int lengthDiff = _heapMax.Length - _heapMin.Length;

        if (lengthDiff is 2)
        {
            _heapMin.Insert(_heapMax.Extract());
        }
        else if(lengthDiff is -2)
        {
            _heapMax.Insert(_heapMin.Extract());
        }
    }

    public T Current { get => _heapMax.Length >= _heapMin.Length ? _heapMax.Extremum : _heapMin.Extremum; }
}
