
namespace DataStructures.Lists;

public static class Extensions
{
    public static SequentialList<T> ToSequentialList<T>(this IEnumerable<T> enumerable)
    {
        SequentialList<T> sequentialList = new();

        if (enumerable is null)
            throw new ArgumentNullException(nameof(enumerable));

        foreach (var item in enumerable)
        {
            sequentialList.Add(item);
        }

        return sequentialList;
    }

    public static void ToSequentialList<T>(this IEnumerable<T> enumerable, SequentialList<T> sequentialList, int count)
    {
        if (enumerable is null)
            throw new ArgumentNullException(nameof(enumerable));

        if (sequentialList is null)
            throw new ArgumentNullException(nameof(sequentialList));

        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        foreach (var item in enumerable)
        {
            sequentialList.Add(item);

            if (sequentialList.Count == count)
                break;
        }
    }

    public static void ToSequentialList<T>(this IEnumerable<T> enumerable, SequentialList<T> sequentialList, Func<T, bool> predicate)
    {
        if (enumerable is null)
            throw new ArgumentNullException(nameof(enumerable));

        if (sequentialList is null)
            throw new ArgumentNullException(nameof(sequentialList));

        if (predicate is null)
            throw new ArgumentNullException(nameof(predicate));

        foreach (var item in enumerable)
        {
            if (predicate(item))
                sequentialList.Add(item);
        }
    }
}
