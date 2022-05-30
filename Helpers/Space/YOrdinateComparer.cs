using Models.Space;
using System.Collections.Generic;

namespace Helpers.Space;

public class YOrdinateComparer<T> : IComparer<T> where T : notnull, IPoint2D
{
    public int Compare(T p1, T p2)
    {
        if (p1.GetY().CompareTo(p2.GetY()) != 0)
        {
            return p1.GetY().CompareTo(p2.GetY());
        }
        else
        {
            return 0;
        }
    }
}