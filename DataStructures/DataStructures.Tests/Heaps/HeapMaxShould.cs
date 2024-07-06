namespace DataStructures.Tests.Heaps;

public class HeapMaxShould
{
    [Fact]
    public void Indexer_WhenHeapFilled_ShouldReturnNode()
    {
        var heap = new HeapMax<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap.Extremum.Key.Should().Be(3);
        heap.Extremum.Value.Should().Be("three");
    }

    [Fact]
    public void Count_WhenHeapFilled_ShouldReturnNumberOfNodes()
    {
        var heap = new HeapMax<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap.Length.Should().Be(3);
    }

    [Fact]
    public void Empty_WhenHeapEmpty_ShouldReturnTrue()
    {
        var heap = new HeapMax<int, string>();

        heap.Empty.Should().BeTrue();
    }

    [Fact]
    public void ExtractKey_WhenHeapFilled_ShouldReturnMaximumKey()
    {
        var heap = new HeapMax<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        var key = heap.ExtractKey();

        key.Should().Be(3);
    }

    [Fact]
    public void ExtractNode_WhenHeapFilled_ShouldReturnMaximumNode()
    {
        var heap = new HeapMax<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        var key = heap.ExtractNode();

        key.Key.Should().Be(3);
    }
}
