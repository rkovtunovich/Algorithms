using System.Diagnostics.CodeAnalysis;

namespace Graphs.Model;

public class Vertex : IEquatable<Vertex>
{
    public Vertex(int index)
    {
        Index = index;
        Distance = null;
    }

    public int Index { get; set; }

    public double? ServiceValue { get; set; }

    public double? Distance { get; set; }

    public int? Component { get; set; }

    public double? LocalClusteringCoefficient { get; set; } = null;

    public double? Betweenness { get; set; } = null;

    public string? Label { get; set; }

    public double? Weight { get; set; } = null;

    public bool Mark { get; set; }

    #region Equality

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is null)
            return false;

        return Equals((Vertex)obj);
    }

    public bool Equals(Vertex? other)
    {
        return other is not null && Index == other.Index;
    }

    public override int GetHashCode()
    {
        return Index;
    }

    public static bool operator ==(Vertex left, Vertex right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vertex left, Vertex right)
    {
        return !(left == right);
    }

    #endregion

    public int ArrayIndex()
    {
        return Index - 1;
    }

    public override string ToString()
    {
        var name = $"\"{Index}";

        if (Mark)
            name += $"\n\u2605";

        if (Label is not null)
            name += $"\n{Label}";

        if (Component is not null)
            name += $"\ncomp:{Component}";

        if (Distance is not null)
            name += $"\nd={Distance:0.00}";

        if (LocalClusteringCoefficient is not null)
            name += $"\nlcc:{LocalClusteringCoefficient:0.00}";

        if (Betweenness is not null)
            name += $"\nbetweenness:{Betweenness:0.00}";

        if (Weight is not null)
            name += $"\nweight:{Weight:0.00}";

        name += "\"";

        return name;
    }

    public void CopyFrom(Vertex? source)
    {
        if (source is null)
            return;

        var type = typeof(Vertex);
        foreach (var sourceProperty in type.GetProperties())
        {
            if (sourceProperty.Name is nameof(Index))
                continue;

            var targetProperty = type.GetProperty(sourceProperty.Name);
            targetProperty?.SetValue(this, sourceProperty.GetValue(source, null), null);
        }
    }
}
