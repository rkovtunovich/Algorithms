namespace DataStructures.Lists;

public class SequentialQueue<T>
{
    private T[] array;
    private int head;
    private int tail;

    public SequentialQueue(int capacity)
    {
        array = new T[capacity];
        head = 0;
        tail = 0;
    }

    public void Enqueue(T item)
    {
        if (tail == array.Length)
        {
            throw new InvalidOperationException("Queue is full.");
        }
        array[tail] = item;
        tail++;
    }

    public T Dequeue()
    {
        if (head == tail)
        {
            throw new InvalidOperationException("Queue is empty.");
        }
        T item = array[head];
        head++;
        return item;
    }

    public T Peek()
    {
        if (head == tail)
        {
            throw new InvalidOperationException("Queue is empty.");
        }
        return array[head];
    }

    public int Count
    {
        get { return tail - head; }
    }
}
