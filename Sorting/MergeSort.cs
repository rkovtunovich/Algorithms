
namespace Sorting
{
    public static class MergeSort
    {
        public static void Sort(ref int[] array)
        {
            int[] arr = SortArray(array, 0, array.Length - 1);

            array = arr;
        }

        private static int[] SortArray(int[] array, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
                return new int[] { array[startIndex] }; 

            int midlle = (endIndex + startIndex) / 2;
            int[] firstHalf = SortArray(array, startIndex, midlle);
            int[] secondHalf = SortArray(array, midlle + 1, endIndex);

            return Merge(firstHalf, secondHalf);
        }

        private static int[] Merge(int[] leftHalf, int[] rightHalf) 
        {
            int length = leftHalf.Length + rightHalf.Length;
            int[] result = new int[length];
            
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
                else if ( leftHalf[currLeft] <= rightHalf[currRight] )
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
