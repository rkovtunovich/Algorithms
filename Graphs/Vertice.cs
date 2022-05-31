namespace Graphs;

public struct Vertice<T> where T : INumber<T>
{
    public int Index { get; set; }

    public T? Value { get; set; }

    public string Label { get; set; }

    public override string ToString()
    {
        return Index.ToString();
    }
}
