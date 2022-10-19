using System.Numerics;

namespace DataStructures;

public class HeapMin<T> : Heap<T> where T : INumber<T>
{
    protected override void HeapifyUp(int position)
    {
        if (position is 1)
            return;

        int parent = GetParentPosition(position);

        if (this[parent] <= this[position])
            return;

        (this[parent], this[position]) = (this[position], this[parent]);

        HeapifyUp(parent);
    }

    protected override void HeapifyDown(int position)
    {
        int leftChildPos = GetLeftChildPosition(position);
        int rightChildPos = GetRightChildPosition(position);

        int nextPos = GetHeapifyDownPosition(position, leftChildPos, rightChildPos);

        if (nextPos == position)
            return;

        (this[position], this[nextPos]) = (this[nextPos], this[position]);
        HeapifyDown(nextPos);
    }

    private int GetHeapifyDownPosition(int parent, int left, int right) => (parent, left, right) switch
    {
        (_, -1, -1) => parent,
        (_, -1, _) when this[right] < this[parent] => right,
        (_, -1, _) when this[right] >= this[parent] => parent,
        (_, _, -1) when this[left] < this[parent] => left,
        (_, _, -1) when this[left] >= this[parent] => parent,
        (_, _, _) when this[left] < this[parent] && this[left] <= this[right] => left,
        (_, _, _) when this[right] < this[parent] && this[right] < this[left] => right,
        (_, _, _) => parent
    };
}
