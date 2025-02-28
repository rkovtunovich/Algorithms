namespace DataStructures.HashTables.OpenAddressing;

// A simple hash set implemented with open addressing and a free list.
public class SimpleHashSet<T> : IHashTable<T> where T : notnull
{
    private const int InitialCapacity = 16;

    // When the table is more than this percent full (active entries),
    // we trigger a resize.
    private const int FillingLimitPercent = 70;

    // Array holding all entries.
    private Entry<T>[] _table;

    // Count of active entries.
    private int _count = 0;

    // Head of the free list (indices of deleted slots). -1 means no free slot.
    private int _freeListHead = -1;

    public SimpleHashSet()
    {
        _table = new Entry<T>[InitialCapacity];
        for (int i = 0; i < _table.Length; i++)
            _table[i] = new Entry<T>();
    }

    public SimpleHashSet(int capacity)
    {
        _table = new Entry<T>[capacity];
        for (int i = 0; i < _table.Length; i++)
            _table[i] = new Entry<T>();
    }

    // Adds a value to the set.
    public void Add(T value)
    {
        // Resize if needed.
        if (NeedResize())
            ResizeTable();

        int index = GetIndex(value);
        int startIndex = index;
        // We record the first encountered deleted slot (from the free list) in the probe sequence.
        int freeIndex = -1;

        while (true)
        {
            var entry = _table[index];
            if (entry.Status is EntryStatus.NeverUsed)
            {
                // If we encountered a deleted slot earlier, use that.
                if (freeIndex is not -1)
                    index = freeIndex;

                _table[index].Value = value;
                _table[index].Status = EntryStatus.Active;
                _count++;
                // If we used a slot from the free list, update the free list head.
                if (freeIndex != -1)
                {
                    // Our free list is maintained by linking deleted indices.
                    // Remove the used index from the free list.
                    _freeListHead = _table[freeIndex].NextFree;
                    _table[freeIndex].NextFree = -1;
                }

                return;
            }
            else if (entry.Status is EntryStatus.Deleted)
            {
                // Save the first deleted slot we see.
                if (freeIndex is -1)
                    freeIndex = index;
            }
            else if (entry.Status is EntryStatus.Active)
            {
                if (entry.Value.Equals(value))
                {
                    // Value already present—do nothing.
                    return;
                }
            }
            index = (index + 1) % _table.Length;
            if (index == startIndex)
                throw new Exception("Hash table is full.");
        }
    }

    // Checks whether the set contains the given value.
    public bool Contains(T value)
    {
        int index = GetIndex(value);
        int startIndex = index;

        while (true)
        {
            var entry = _table[index];
            if (entry.Status is EntryStatus.NeverUsed)
                return false;  // Value not found.

            if (entry.Status is EntryStatus.Active && entry.Value.Equals(value))
                return true;

            index = (index + 1) % _table.Length;
            if (index == startIndex)
                return false;
        }
    }

    // Removes the value from the set.
    public void Remove(T value)
    {
        int index = GetIndex(value);
        int startIndex = index;

        while (true)
        {
            var entry = _table[index];
            if (entry.Status is EntryStatus.NeverUsed)
                return;  // Value not found.

            if (entry.Status is EntryStatus.Active && entry.Value.Equals(value))
            {
                // Mark this slot as deleted.
                entry.Status = EntryStatus.Deleted;
                // Link this slot into the free list.
                entry.NextFree = _freeListHead;
                _freeListHead = index;
                _count--;
                return;
            }

            index = (index + 1) % _table.Length;
            if (index == startIndex)
                return;
        }
    }

    // Returns the current number of active entries.
    public int Length => _count;

    // Computes the initial bucket index from the value's hash code.
    private int GetIndex(T value)
    {
        int hash = value.GetHashCode();
        return Math.Abs(hash) % _table.Length;
    }

    // Determines whether the table needs resizing based on load.
    private bool NeedResize()
    {
        return _count * 100 / _table.Length > FillingLimitPercent;
    }

    // Resizes the table to double its current capacity and rehashes active entries.
    private void ResizeTable()
    {
        var oldTable = _table;
        int newCapacity = oldTable.Length * 2;
        _table = new Entry<T>[newCapacity];
        for (int i = 0; i < newCapacity; i++)      
            _table[i] = new Entry<T>();
        
        // Reset count and free list.
        _count = 0;
        _freeListHead = -1;

        // Reinsert active entries.
        foreach (var entry in oldTable)
            if (entry.Status is EntryStatus.Active)
                Add(entry.Value);
    }

    #region IEnumerable Implementation 

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var entry in _table)
            if (entry.Status is EntryStatus.Active)
                yield return entry.Value;

    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    #endregion
}
