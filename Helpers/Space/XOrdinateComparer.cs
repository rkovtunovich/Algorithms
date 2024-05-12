using Models.Spacing;

namespace Helpers.Space;

public class XOrdinateComparer<T> : IComparer<T> where T : notnull, IPoint
{
    public int Compare(T p1, T p2)
    {
        if (p1.GetCoordinate(0).CompareTo(p2.GetCoordinate(0)) != 0)
        {
            return p1.GetCoordinate(0).CompareTo(p2.GetCoordinate(0));
        }
        else
        {
            return 0;
        }
    }
}