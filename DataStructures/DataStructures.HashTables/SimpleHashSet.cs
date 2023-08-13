namespace DataStructures.HashTables;

// // A simple hash set implementation using a hash table.
public class SimpleHashSet<T> : IHashTable<T> where T : notnull
{
    // Default initial length for the bucket array.
    private const int InitialLength = 16;

    // Limit for resizing the bucket array. When the array's filling percentage exceeds this limit, it'll be resized.
    private const int FillingLimitPercent = 70;

    // Array to hold the hash table's buckets.
    private Entry<T>?[] _buckets;

    // Holds the current number of elements in the hash set.
    private int _length = 0;

    // Default constructor: Initializes the buckets array with a default size.
    public SimpleHashSet()
    {
        _buckets = new Entry<T>[InitialLength];
    }

    // Constructor: Initializes the buckets array with a given size.
    public SimpleHashSet(int length)
    {
        _buckets = new Entry<T>[length];
    }

    // Adds a value to the hash set.
    public void Add(T value)
    {
        _length++;

        int index = GetIndex(value);

        if (_buckets[index] is null)
            _buckets[index] = new Entry<T>()
            {
                Value = value
            };
        else
            AddRecursively(_buckets[index], value);

        ResizeBackets();
    }

    // Checks if the hash set contains a given value.
    public bool Contains(T value)
    {
        var entrie = _buckets[GetIndex(value)];

        if (entrie is null)
            return false;

        return SearchRecursively(entrie, value);
    }

    // Checks if the hash set contains a given value.
    public void Remove(T value)
    {
        int index = GetIndex(value);
        var entry = _buckets[index];
        if (entry is null)
            return;

        if (entry.Value.Equals(value))
        {
            _length--;

            if (entry.Next is null)
                _buckets[index] = null;
            else
                _buckets[index] = entry.Next;

            return;
        }

        RemoveRecursively(entry, value);
    }

    // Returns the number of elements in the hash set.
    public int Length { get => _length; }

    // Loads elements from a linked list into the hash set.
    public void Load(LinkedList<T> list)
    {
        foreach (var item in list)
        {
            Add(item);
        }
    }

    #region IEnumerable

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    #endregion

    // These private methods support the basic functionality of the hash set.
    #region Service methods

    // Calculates the index in the bucket array for a given value.
    private int GetIndex(T value)
    {
        var hashCode = value.GetHashCode();
        int index = Math.Abs(hashCode % _buckets.Length);

        return index;
    }

    // Recursively adds a value to the linked list within a bucket.
    private void AddRecursively(Entry<T> entry, T value)
    {
        if (entry.Next is null)
            entry.Next = new Entry<T>() { Value = value };
        else
            AddRecursively(entry.Next, value);
    }

    // Recursively adds a value to the linked list within a bucket.
    private bool SearchRecursively(Entry<T> entry, T value)
    {
        if (entry.Value.Equals(value))
            return true;
        else if (entry.Next is null)
            return false;
        else
            return SearchRecursively(entry.Next, value);
    }

    // Recursively removes a value from the linked list within a bucket.
    private void RemoveRecursively(Entry<T> parentEntry, T value)
    {
        if (parentEntry.Next is null)
            return;

        if (parentEntry.Next.Value.Equals(value))
        {
            _length--;
            parentEntry.Next = null;
            return;
        }

        RemoveRecursively(parentEntry.Next, value);
    }

    // Resizes the bucket array if it's above the filling limit.
    private void ResizeBackets()
    {
        if (_buckets.Length == 0)
            return;

        var filling = _length * 100 / _buckets.Length;

        if (filling > FillingLimitPercent)
        {
            var prevBackets = _buckets;
            _buckets = new Entry<T>[prevBackets.Length * 2];
            _length = 0;

            for (int i = 0; i < prevBackets.Length; i++)
            {
                CopyEntriesRecursively(prevBackets[i]);
            }
        }
    }

    // Copies entries from one bucket array to another, used during resizing.
    private void CopyEntriesRecursively(Entry<T>? entry)
    {
        if (entry is null)
            return;

        Add(entry.Value);

        if (entry.Next is not null)
            CopyEntriesRecursively(entry.Next);
    }

    #endregion
}
