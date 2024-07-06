namespace DataStructures.Tests.Heaps;

public class DaryMaxHeapShould
{
    [Fact]
    public void Insert_WhenHeapEmpty_ShouldInsertNode()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        // Act
        heap.Insert(1, "one");

        // Assert
        heap.Length.Should().Be(1);
    }

    [Fact]
    public void Insert_WhenHeapFilled_ShouldInsertNode()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");
        heap.Insert(4, "four");

        // Act
        heap.Insert(5, "five");

        // Assert
        heap.Length.Should().Be(5);
        heap.Extremum.Key.Should().Be(5);
        heap.Extremum.Value.Should().Be("five");
    }

    [Fact]
    public void Insert_WhenHeapFilled_ShouldInsertNodeAtCorrectPosition()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");
        heap.Insert(4, "four");
        heap.Insert(5, "five");

        // Act
        heap.Insert(0, "zero");
        heap.Insert(6, "six");

        // Assert
        heap.Length.Should().Be(7);
        heap.Extremum.Key.Should().Be(6);
        heap.Extremum.Value.Should().Be("six");
        heap[1].Key.Should().Be(5);
        heap[1].Value.Should().Be("five");
        heap[2].Key.Should().Be(2);
        heap[2].Value.Should().Be("two");
        heap[3].Key.Should().Be(3);
        heap[3].Value.Should().Be("three");
        heap[4].Key.Should().Be(1);
        heap[4].Value.Should().Be("one");
        heap[5].Key.Should().Be(0);
        heap[5].Value.Should().Be("zero");
        heap[6].Key.Should().Be(4);
        heap[6].Value.Should().Be("four");

    }

    [Fact]
    public void ExtractKey_WhenHeapFilled_ShouldReturnMinimumKey()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");
        heap.Insert(4, "four");
        heap.Insert(5, "five");

        // Act
        var key = heap.ExtractKey();

        // Assert
        key.Should().Be(5);
    }

    [Fact]
    public void ExtractNode_WhenHeapFilled_ShouldReturnMinimumNode()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");
        heap.Insert(4, "four");
        heap.Insert(5, "five");

        // Act
        var key = heap.ExtractNode();

        // Assert
        key.Key.Should().Be(5);
    }

    [Fact]
    public void ReplaceKeyByValue_WhenHeapFilled_ShouldReplaceKeyByValue()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");
        heap.Insert(4, "four");
        heap.Insert(5, "five");

        // Act
        heap.ReplaceKeyByValue("five", 0);

        // Assert
        heap.Length.Should().Be(5);
        heap.Extremum.Key.Should().Be(4);
        heap.Extremum.Value.Should().Be("four");
        heap[1].Key.Should().Be(1);
        heap[1].Value.Should().Be("one");
        heap[2].Key.Should().Be(2);
        heap[2].Value.Should().Be("two");
        heap[3].Key.Should().Be(3);
        heap[3].Value.Should().Be("three");
        heap[4].Key.Should().Be(0);
        heap[4].Value.Should().Be("five");
    }

    [Fact]
    public void ReplaceKey_WhenHeapFilled_ShouldReplaceKey()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");
        heap.Insert(4, "four");
        heap.Insert(5, "five");

        // Act
        heap.ReplaceKey(5, 0);

        // Assert
        heap.Length.Should().Be(5);
        heap.Extremum.Key.Should().Be(4);
        heap.Extremum.Value.Should().Be("four");
        heap[1].Key.Should().Be(1);
        heap[1].Value.Should().Be("one");
        heap[2].Key.Should().Be(2);
        heap[2].Value.Should().Be("two");
        heap[3].Key.Should().Be(3);
        heap[3].Value.Should().Be("three");
        heap[4].Key.Should().Be(0);
        heap[4].Value.Should().Be("five");
    }

    [Fact]
    public void RemoveByValue_WhenHeapFilled_ShouldRemoveNodeByValue()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");
        heap.Insert(4, "four");
        heap.Insert(5, "five");

        // Act
        heap.RemoveByValue("five");

        // Assert
        heap.Length.Should().Be(4);
        heap.Extremum.Key.Should().Be(4);
        heap.Extremum.Value.Should().Be("four");
        heap[1].Key.Should().Be(1);
        heap[1].Value.Should().Be("one");
        heap[2].Key.Should().Be(2);
        heap[2].Value.Should().Be("two");
        heap[3].Key.Should().Be(3);
        heap[3].Value.Should().Be("three");
    }

    [Fact]
    public void RemoveByKey_WhenHeapFilled_ShouldRemoveNodeByKey()
    {
        // Arrange
        var heap = new DaryMaxHeap<int, string>(3);

        heap.Insert(1, "one");
        heap.Insert(2, "two");
        heap.Insert(3, "three");
        heap.Insert(4, "four");
        heap.Insert(5, "five");

        // Act
        heap.RemoveByKey(5);

        // Assert
        heap.Length.Should().Be(4);
        heap.Extremum.Key.Should().Be(4);
        heap.Extremum.Value.Should().Be("four");
        heap[1].Key.Should().Be(1);
        heap[1].Value.Should().Be("one");
        heap[2].Key.Should().Be(2);
        heap[2].Value.Should().Be("two");
        heap[3].Key.Should().Be(3);
        heap[3].Value.Should().Be("three");
    }
}
