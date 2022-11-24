﻿namespace DataStructures.HashTables;

public interface IHashTable<T> where T: notnull
{
    public void Add(T value);

    public bool Contains(T value);

    public void Remove(T value);
}
