namespace DataStructures.HashTables.OpenAddressing;

// Status for each slot in the table.
public enum EntryStatus
{
    NeverUsed,  // Slot has never been used.
    Active,     // Slot currently holds an element.
    Deleted     // Slot held an element that has been removed.
}
