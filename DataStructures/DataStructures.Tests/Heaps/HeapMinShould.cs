namespace DataStructures.Tests.Heaps;

public class HeapMinShould
{
    [Fact]
    public void Indexer_WhenHeapFilled_ShouldReturnNode()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap[0].Key.Should().Be(1);
        heap[0].Value.Should().Be("one");
    }

    [Fact]
    public void Count_WhenHeapFilled_ShouldReturnNumberOfNodes()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap.Length.Should().Be(3);
    }

    [Fact]
    public void Empty_WhenHeapEmpty_ShouldReturnTrue()
    {
        var heap = new HeapMin<int, string>();

        heap.Empty.Should().BeTrue();
    }

    [Fact]
    public void ExtractKey_WhenHeapFilled_ShouldReturnMinimumKey()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        var key = heap.ExtractKey();

        key.Should().Be(1);
    }

    [Fact]
    public void ExtractNode_WhenHeapFilled_ShouldReturnMinimumNode()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        var node = heap.ExtractNode();

        node.Key.Should().Be(1);
        node.Value.Should().Be("one");
    }

    [Fact]
    public void Insert_WhenHeapEmpty_ShouldInsertNode()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");

        heap.Length.Should().Be(1);
        heap.Extremum.Key.Should().Be(1);
        heap.Extremum.Value.Should().Be("one");
    }

    [Fact]
    public void Insert_WhenHeapFilled_ShouldInsertNode()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap.Insert(0, "zero");

        heap.Length.Should().Be(4);
        heap.Extremum.Key.Should().Be(0);
        heap.Extremum.Value.Should().Be("zero");
    }

    [Fact]
    public void ReplaceKey_WhenHeapFilled_ShouldReplaceKey()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap.ReplaceKey(2, 0);

        heap.Length.Should().Be(3);
        heap.Extremum.Key.Should().Be(0);
        heap.Extremum.Value.Should().Be("two");
    }

    [Fact]
    public void ReplaceKeyByValue_WhenHeapFilled_ShouldReplaceKey()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap.ReplaceKeyByValue("two", 0);

        heap.Length.Should().Be(3);
        heap.Extremum.Key.Should().Be(0);
        heap.Extremum.Value.Should().Be("two");
    }

    [Fact]
    public void RemoveByValue_WhenHeapFilled_ShouldRemoveNode()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap.RemoveByValue("two");

        heap.Length.Should().Be(2);
        heap[0].Key.Should().Be(1);
        heap[0].Value.Should().Be("one");
    }

    [Fact]
    public void RemoveByKey_WhenHeapFilled_ShouldRemoveNode()
    {
        var heap = new HeapMin<int, string>();

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        heap.RemoveByKey(2);

        heap.Length.Should().Be(2);
        heap.Extremum.Key.Should().Be(1);
        heap.Extremum.Value.Should().Be("one");
    }

    [Fact]
    public void GetLeftChildPosition_WhenCalled_ReturnsLeftChildPosition()
    {
        // Arrange
        var heap = new HeapMin<int, string>();
        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        // Act
        var leftChildPosition = heap.GetLeftChildPosition(1);

        // Assert
        leftChildPosition.Should().Be(2);
    }

    [Fact]
    public void GetRightChildPosition_WhenCalled_ReturnsRightChildPosition()
    {
        // Arrange
        var heap = new HeapMin<int, string>();
        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");

        // Act
        var rightChildPosition = heap.GetRightChildPosition(1);

        // Assert
        rightChildPosition.Should().Be(3);
    }
}
