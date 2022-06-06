using System.Diagnostics.CodeAnalysis;

namespace Graphs;

public class Vertice<T>: IEquatable<Vertice<T>>
{
    public Vertice(int index)
    {
        Index = index;
        Value = null;
    }

    public int Index { get; set; }

    public T? Data { get; set; }

    public double? Value { get; set; }

    public int? Component { get; set; }

    public string? Label { get; set; }

    #region Equality

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is null)
            return false;

        return Equals((Vertice<T>)obj);
    }

    public bool Equals(Vertice<T>? other)
    {
        return other is not null && Index == other.Index;
    }

    public override int GetHashCode()
    {
        return Index;
    }

    public static bool operator ==(Vertice<T> left, Vertice<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vertice<T> left, Vertice<T> right)
    {
        return !(left == right);
    }

    #endregion

    public override string ToString()
    {
        return $"\"{Component}  {Index}:{Value}\"";
    }

}
