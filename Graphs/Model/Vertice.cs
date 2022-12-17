using System.Diagnostics.CodeAnalysis;

namespace Graphs.Model;

public class Vertice : IEquatable<Vertice>
{
    public Vertice(int index)
    {
        Index = index;
        Distance = null;
    }

    public int Index { get; set; }

    public double? ServiceValue { get; set; }

    public double? Distance { get; set; }

    public int? Component { get; set; }

    public double? LocalClusteringCoefficient { get; set; } = null;

    public double? Betweeness { get; set; } = null;

    public string? Label { get; set; }

    public double? Weight { get; set; } = null;

    public bool Mark { get; set; }

    #region Equality

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is null)
            return false;

        return Equals((Vertice)obj);
    }

    public bool Equals(Vertice? other)
    {
        return other is not null && Index == other.Index;
    }

    public override int GetHashCode()
    {
        return Index;
    }

    public static bool operator ==(Vertice left, Vertice right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vertice left, Vertice right)
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

        if (Betweeness is not null)
            name += $"\nbetweeness:{Betweeness:0.00}";

        if (Weight is not null)
            name += $"\nweight:{Weight:0.00}";

        name += "\"";

        return name;
    }

    public void CopyFrom(Vertice? sourse)
    {
        if (sourse is null)
            return;

        var type = typeof(Vertice);
        foreach (var sourceProperty in type.GetProperties())
        {
            if (sourceProperty.Name is nameof(Index))
                continue;

            var targetProperty = type.GetProperty(sourceProperty.Name);
            targetProperty?.SetValue(this, sourceProperty.GetValue(sourse, null), null);
        }
    }
}
