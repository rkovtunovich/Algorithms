namespace DataStructures.HashTables.OpenAddressing;

// Entry stored in the open-addressing table.
public class Entry<T>
{
    public T Value;
    public EntryStatus Status = EntryStatus.NeverUsed;
    // NextFree is used only when the slot is marked as Deleted.
    public int NextFree = -1;
}
