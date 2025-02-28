namespace DataStructures.HashTables.SeparateChaining;

public class Entry<T> where T : notnull
{
    public Entry<T>? Next { get; set; }

    public T Value { get; init; }
}
