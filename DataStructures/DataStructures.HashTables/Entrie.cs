namespace DataStructures.HashTables;

public class Entrie<T> where T : notnull
{
    public Entrie<T>? Next { get; set; }

    public T Value { get; init; }
}
