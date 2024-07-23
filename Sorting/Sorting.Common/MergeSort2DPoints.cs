using Spacing.Core.Points;

namespace Sorting.Common;

public static class MergeSort2DPoints
{
    public static void Sort(ref Point2D[] array, int dimension)
    {
        Point2D[] arr = SortArray(array, 0, array.Length - 1, dimension);

        array = arr;
    }

    private static Point2D[] SortArray(Point2D[] array, int startIndex, int endIndex, int dimension)
    {
        if (startIndex == endIndex)
            return [array[startIndex]];

        int middle = (endIndex + startIndex) / 2;
        Point2D[] firstHalf = SortArray(array, startIndex, middle, dimension);
        Point2D[] secondHalf = SortArray(array, middle + 1, endIndex, dimension);

        return Merge(firstHalf, secondHalf, dimension);
    }

    private static Point2D[] Merge(Point2D[] leftHalf, Point2D[] rightHalf, int dimension)
    {
        int length = leftHalf.Length + rightHalf.Length;
        Point2D[] result = new Point2D[length];

        int currLeft = 0;
        int currRight = 0;
        for (int i = 0; i < length; i++)
        {
            if (currLeft == leftHalf.Length)
            {
                result[i] = rightHalf[currRight];
                currRight++;
            }
            else if (currRight == rightHalf.Length)
            {
                result[i] = leftHalf[currLeft];
                currLeft++;
            }
            else if (leftHalf[currLeft].CompareByDimension(dimension, rightHalf[currRight]) <= 0)
            {
                result[i] = leftHalf[currLeft];
                currLeft++;
            }
            else
            {
                result[i] = rightHalf[currRight];
                currRight++;
            }
        }

        return result;
    }
}
