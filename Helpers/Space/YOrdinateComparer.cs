using Models.Spacing;

namespace Helpers.Space;

public class YOrdinateComparer<T> : IComparer<T> where T : notnull, IPoint
{
    public int Compare(T p1, T p2)
    {
        if (p1.GetCoordinate(0).CompareTo(p2.GetCoordinate(1)) != 0)
            return p1.GetCoordinate(1).CompareTo(p2.GetCoordinate(1));
        else
            return 0;
    }
}