using System.Diagnostics.CodeAnalysis;

namespace Graphs;

public class Vertice<T>: IEquatable<Vertice<T>>
{
    public Vertice(int index)
    {
        Index = index;
        Distance = null;
    }

    public int Index { get; set; }

    public T? Data { get; set; }

    public double? Distance { get; set; }

    public int? Component { get; set; }

    public double? LocalClusteringCoefficient { get; set; } = null;

    public double? Betweeness { get; set; } = null;

    public string? Label { get; set; }

    public double? Weight { get; set; } = null;

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
        var name = $"\"index:{Index}";

        if (Component is not null)
            name += $"\ncomp:{Component}";

        if (Distance is not null)
            name += $"\ndist:{Distance}";

        if (LocalClusteringCoefficient is not null)
            name += $"\nlcc:{LocalClusteringCoefficient:0.00}";

        if (Betweeness is not null)
            name += $"\nbetweeness:{Betweeness:0.00}";

        if (Weight is not null)
            name += $"\nweight:{Weight:0.00}";

        name += "\"";

        return name;
    }
}
