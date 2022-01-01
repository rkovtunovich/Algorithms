
using Models.Space;
using System;
using System.Collections.Generic;

namespace Sorting
{
    public static class MergeSort2DPoints
    {
        private static IComparer<Point2D> _comparer;

        public static void Sort(ref Point2D[] array, IComparer<Point2D> comparer)
        {
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));

            _comparer = comparer;

            Point2D[] arr = SortArray(array, 0, array.Length - 1);

            array = arr;
        }

        private static Point2D[] SortArray(Point2D[] array, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
                return new Point2D[] { array[startIndex] }; 

            int midlle = (endIndex + startIndex) / 2;
            Point2D[] firstHalf = SortArray(array, startIndex, midlle);
            Point2D[] secondHalf = SortArray(array, midlle + 1, endIndex);

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
                //else if ( leftHalf[currLeft] <= rightHalf[currRight] )
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
