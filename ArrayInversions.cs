
namespace Sorting
{
    internal class ArrayInversions
    {
        public static int Count(ref int[] array)
        {
            (array, int count) = CountInversions(array, 0, array.Length - 1);

            return count;
        }

        private static (int[] array, int inversions) CountInversions(int[] array, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
                return (new int[] { array[startIndex] }, 0);

            int midlle = (endIndex + startIndex) / 2;

            (int[] leftHalf, int leftInv ) leftResult = CountInversions(array, startIndex, midlle);
            (int[] rightHalf, int rightInv) rightResult = CountInversions(array, midlle + 1, endIndex);

            (int[] merged, int Inv) mergedResult = Merge(leftResult.leftHalf, rightResult.rightHalf);

            mergedResult.Inv += leftResult.leftInv;
            mergedResult.Inv += rightResult.rightInv;

            return mergedResult;
        }

        private static (int[], int) Merge(int[] leftHalf, int[] rightHalf)
        {
            int length = leftHalf.Length + rightHalf.Length;
            int[] result = new int[length];
            int inveriosn = 0;

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
                else if (leftHalf[currLeft] <= rightHalf[currRight])
                {
                    result[i] = leftHalf[currLeft];
                    currLeft++;
                }
                else
                {
                    result[i] = rightHalf[currRight];
                    inveriosn += leftHalf.Length - currLeft;
                    currRight++;                   
                }
            }

            return (result, inveriosn);
        }
    }
}
