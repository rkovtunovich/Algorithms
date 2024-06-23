using Sorting.Common;

namespace Distance.Common;

internal class OneDimension
{
    public static (int, int) GetClosestPoints(int[] setOfPoints)
    {
        MergeSort.Sort(ref setOfPoints);

        var points = (x1: 0, x2: 0);
        if (setOfPoints.Length < 2)
            return points;

        points.x1 = setOfPoints[0];
        points.x2 = setOfPoints[1];

        for (int i = 1; i < setOfPoints.Length - 2; i++)
        {
            if (setOfPoints[i + 1] - setOfPoints[i] < points.x2 - points.x1)
            {
                points.x1 = setOfPoints[i];
                points.x2 = setOfPoints[i + 1];
            }
        }

        return points;
    }
}