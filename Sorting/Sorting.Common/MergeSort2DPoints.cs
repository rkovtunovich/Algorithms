using Models.Spacing;

namespace Sorting.Common
{
    public static class MergeSort2DPoints
    {
        private static IComparer<Point2D> _comparer = null!;

        public static void Sort(ref Point2D[] array, IComparer<Point2D> comparer)
        {
            ArgumentNullException.ThrowIfNull(comparer);

            _comparer = comparer;

            Point2D[] arr = SortArray(array, 0, array.Length - 1);

            array = arr;
        }

        private static Point2D[] SortArray(Point2D[] array, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
                return [array[startIndex]];

            int middle = (endIndex + startIndex) / 2;
            Point2D[] firstHalf = SortArray(array, startIndex, middle);
            Point2D[] secondHalf = SortArray(array, middle + 1, endIndex);

            return Merge(firstHalf, secondHalf);
        }

        private static Point2D[] Merge(Point2D[] leftHalf, Point2D[] rightHalf)
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
                else if (_comparer.Compare(leftHalf[currLeft], rightHalf[currRight]) <= 0)
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
}
