using Models.Space;
using System.Collections.Generic;

namespace Helpers.Space;

public class XOrdinateComparer<T> : IComparer<T> where T : notnull, IPoint
{
    public int Compare(T p1, T p2)
    {
        if (p1.GetX().CompareTo(p2.GetX()) != 0)
        {
            return p1.GetX().CompareTo(p2.GetX());
        }
        else
        {
            return 0;
        }
    }
}