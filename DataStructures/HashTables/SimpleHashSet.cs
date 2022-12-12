namespace DataStructures.HashTables;

public class SimpleHashSet<T> : IHashTable<T> where T : notnull
{
    private const int InitialLength = 16;
    private const int FillingLinitPercents = 70;

    private Entrie<T>?[] _buckets;

    private int _length = 0;

    public SimpleHashSet()
    {
        _buckets = new Entrie<T>[InitialLength];
    }

    public SimpleHashSet(int length)
    {
        _buckets = new Entrie<T>[length];
    }

    public void Add(T value)
    {
        _length++;

        int index = GetIndex(value);

        if (_buckets[index] is null)
            _buckets[index] = new Entrie<T>(){ 
                Value = value 
            };
        else
            AddRecursevely(_buckets[index], value);

        ResizeBackets();
    }

    public bool Contains(T value)
    {
        var entrie = _buckets[GetIndex(value)];

        if (entrie is null)
            return false;

        return SearchRecursevely(entrie, value);
    }

    public void Remove(T value)
    {
        int index = GetIndex(value);
        var entry = _buckets[index];
        if (entry is null) 
            return;

        if (entry.Value.Equals(value))
        {
            _length--;
            
            if(entry.Next is null)
                _buckets[index] = null;
            else
                _buckets[index] = entry.Next;

            return;
        }
           
        RemoveRecurselevely(entry, value);
    }

    public int Length { get => _length; }

    public void Load(LinkedList<T> list)
    {
        foreach (var item in list)
        {
            Add(item);
        }
    }

    #region Service methods

    private int GetIndex(T value)
    {
        var hashCode = value.GetHashCode();
        int index = Math.Abs(hashCode % _buckets.Length);

        return index;
    }

    private void AddRecursevely(Entrie<T> entrie, T value)
    {
        if (entrie.Next is null)
            entrie.Next = new Entrie<T>() { Value = value };
        else
            AddRecursevely(entrie.Next, value);
    }

    private bool SearchRecursevely(Entrie<T> entrie, T value)
    {
        if (entrie.Value.Equals(value))
            return true;
        else if (entrie.Next is null)
            return false;
        else
            return SearchRecursevely(entrie.Next, value);
    }

    private void RemoveRecurselevely(Entrie<T> parentEntrie, T value)
    {
        if(parentEntrie.Next is null)
            return;

        if (parentEntrie.Next.Value.Equals(value))
        {
            _length--;
            parentEntrie.Next = null;
            return;
        }

        RemoveRecurselevely(parentEntrie.Next, value);
    }

    private void ResizeBackets()
    {
        if(_buckets.Length == 0 )
            return;

        var filling = _length * 100 / _buckets.Length;
        
        if(filling > FillingLinitPercents)
        {
            var prevBackets = _buckets;
            _buckets = new Entrie<T>[prevBackets.Length * 2];
            _length = 0;

            for (int i = 0; i < prevBackets.Length; i++)
            {
                CopyEntriesRecursevely(prevBackets[i]);    
            }
        }
    }

    private void CopyEntriesRecursevely(Entrie<T>? entrie)
    {
        if(entrie is null)
            return;

        Add(entrie.Value);

        if(entrie.Next is not null)
            CopyEntriesRecursevely(entrie.Next);
    }

    #endregion
}
