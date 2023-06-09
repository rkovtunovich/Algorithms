namespace Sorting;

// ShellSort, also known as Shell sort or Shell's method, is an in-place comparison-based sorting algorithm invented by Donald Shell in 1959.
// It is a generalization of the insertion sort algorithm, which improves its performance by allowing the exchange of items that are far apart in the list.
// The key concept behind ShellSort is to rearrange the list in multiple steps, gradually reducing the distance (or gap) between elements being compared.
// 
// ShellSort works as follows:
// 
// 1. Choose a gap sequence: This sequence determines the distance between elements being compared.
//    There are various methods to generate gap sequences, such as the original Shell sequence (N/2, N/4, ..., 1), the Ciura sequence, and the Knuth sequence.
//    The choice of a gap sequence can significantly impact the performance of the algorithm.
// 2. Sort sublists for each gap: For each gap in the sequence, sort the sublists of elements separated by that gap using a basic insertion sort.
//    In each pass, elements closer together are compared and swapped if necessary.
// 3. Reduce the gap: Decrease the gap according to the chosen gap sequence, and repeat step 2 until the gap is 1, which corresponds to a standard insertion sort.
// 4. Final insertion sort: Perform a final insertion sort with a gap of 1 to ensure the list is completely sorted.
//
// Shell sort divides the data into different groups.
// It first sorts each group and then performs one insertion sort on all the elements, in order to reduce the swapping and moving of data.
// Shell sort is very efficient, but the quality of grouping will hugely impact the algorithm performance.
// Shell sort is faster than bubble sort and insertion sort, but it is slower than quick sort, merge sort and heap sort.
// Shell sort is suitable for situations where the number of data is lower than 5,000 and speed is not very important.
// It is very good for sorting arrays with relatively small amount of data.

public static class ShellSort
{
    public static void Sort<T>(T[] array) where T : IComparable<T>
    {
        int n = array.Length;
        int gap = n / 2;

        while (gap > 0)
        {
            for (int i = gap; i < n; i++)
            {
                int j = i;
                T temp = array[i];

                while (j >= gap && array[j - gap].CompareTo(temp) > 0)
                {
                    array[j] = array[j - gap];
                    j -= gap;
                }

                array[j] = temp;
            }

            gap /= 2;
        }
    }
}

