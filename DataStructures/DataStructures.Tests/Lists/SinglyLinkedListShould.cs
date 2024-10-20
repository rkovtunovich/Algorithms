namespace DataStructures.Tests.Lists;

public class SinglyLinkedListShould
{
    [Fact]
    public void IsEmpty_WhenEmpty_ShouldReturnTrue()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();

        // Act
        var isEmpty = list.IsEmpty;

        // Assert
        isEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsEmpty_WhenNotEmpty_ShouldReturnFalse()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);

        // Act
        var isEmpty = list.IsEmpty;

        // Assert
        isEmpty.Should().BeFalse();
    }

    [Fact]
    public void IsReadOnly_ShouldReturnFalse()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();

        // Act
        var isReadOnly = list.IsReadOnly;

        // Assert
        isReadOnly.Should().BeFalse();
    }

    [Fact]
    public void Count_WhenEmpty_ShouldReturnZero()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();

        // Act
        var count = list.Count;

        // Assert
        count.Should().Be(0);
    }

    [Fact]
    public void Count_WhenNotEmpty_ShouldReturnCount()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        var count = list.Count;

        // Assert
        count.Should().Be(2);
    }


    [Fact]
    public void Add_WhenEmpty_ShouldIncrementCountAndAssignHead()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();

        // Act
        list.Add(1);

        // Assert
        list.Should().ContainSingle();
        list.Head!.Value.Should().Be(1);
    }

    [Fact]
    public void Add_WhenNotEmpty_ShouldIncrementCountAndAssignHead()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);

        // Act
        list.Add(2);

        // Assert
        list.Should().HaveCount(2);
        list.Head!.Value.Should().Be(2);
        list.Head.Next!.Value.Should().Be(1);
    }

    [Fact]
    public void Find_WhenEmpty_ShouldReturnNull()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();

        // Act
        var node = list.Find(1);

        // Assert
        node.Should().BeNull();
    }

    [Fact]
    public void Find_WhenValueExists_ShouldReturnNode()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        var node = list.Find(1);

        // Assert
        node.Should().NotBeNull();
        node!.Value.Should().Be(1);
    }

    [Fact]
    public void RemoveFirst_WhenEmpty_ShouldThrowException()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();

        // Act
        Action act = () => list.RemoveFirst();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void RemoveFirst_WhenNotEmpty_ShouldDecrementCountAndAssignHead()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        list.RemoveFirst();

        // Assert
        list.Should().ContainSingle();
        list.Head!.Value.Should().Be(1);
    }

    [Fact]
    public void Remove_WhenEmpty_ShouldThrowException()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();

        // Act
        Action act = () => list.Remove(new ListNode<int>(1));

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Remove_WhenNodeIsHead_ShouldDecrementCountAndAssignHead()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        list.Remove(list.Head!);

        // Assert
        list.Should().ContainSingle();
        list.Head!.Value.Should().Be(1);
    }

    [Fact]
    public void Remove_WhenNodeIsNotHead_ShouldDecrementCountAndAssignNext()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        list.Remove(list.Head!.Next!);

        // Assert
        list.Should().ContainSingle();
        list.Head!.Value.Should().Be(2);
    }

    [Fact]
    public void Remove_WhenNodeIsNull_ShouldThrowException()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();

        // Act
        Action act = () => list.Remove(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

   
    [Fact]  
    public void Remove_WhenValueExists_ShouldRemoveNodeAndReturnTrue()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        var removed = list.Remove(1);

        // Assert
        removed.Should().BeTrue();
        list.Should().ContainSingle();
        list.Head!.Value.Should().Be(2);
    }

    [Fact]
    public void Remove_WhenValueDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        var removed = list.Remove(3);

        // Assert
        removed.Should().BeFalse();
        list.Should().HaveCount(2);
    }

    [Fact]
    public void Clear_ShouldRemoveAllNodes()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        list.Clear();

        // Assert
        list.Should().BeEmpty();
    }

    [Fact]
    public void Reverse_ShouldReverseList()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // Act
        list.Reverse();

        // Assert
        list.Should().HaveCount(3);
        list.Head!.Value.Should().Be(1);
        list.Head.Next!.Value.Should().Be(2);
        list.Head.Next.Next!.Value.Should().Be(3);
    }

    [Fact]
    public void Contains_WhenValueExists_ShouldReturnTrue()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        var contains = list.Contains(1);

        // Assert
        contains.Should().BeTrue();
    }

    [Fact]
    public void Contains_WhenValueDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        // Act
        var contains = list.Contains(3);

        // Assert
        contains.Should().BeFalse();
    }

    [Fact]
    public void CopyTo_WhenArrayIsHasEnoughSpace_ShouldCopyValues()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);
        var array = new int[2];

        // Act
        list.CopyTo(array, 0);

        // Assert
        array.Should().Equal(2, 1);
    }

    [Fact]
    public void CopyTo_WhenArrayIndexIsInvalid_ShouldThrowException()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        var array = new int[2];

        // Act
        Action act = () => list.CopyTo(array, 2);

        // Assert
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void CopyTo_WhenArrayIsNotLargeEnough_ShouldThrowException()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        list.Add(2);

        var array = new int[1];

        // Act
        Action act = () => list.CopyTo(array, 0);

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void UnionWith_WhenOtherIsEmpty_ShouldDoNothing()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        var other = new SinglyLinkedList<int>();
        
        // Act
        list.UnionWith(other);

        // Assert
        list.Should().HaveCount(1);
    }

    [Fact]
    public void UnionWith_WhenEmpty_ShouldAssignHeadAndCount()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        var other = new SinglyLinkedList<int>();
        other.Add(1);
        other.Add(2);

        // Act
        list.UnionWith(other);

        // Assert
        list.Should().HaveCount(2);
        list.Head!.Value.Should().Be(2);
        list.Head.Next!.Value.Should().Be(1);
    }

    [Fact]
    public void UnionWith_WhenNotEmpty_ShouldAssignHeadAndCount()
    {
        // Arrange
        var list = new SinglyLinkedList<int>();
        list.Add(1);
        var other = new SinglyLinkedList<int>();
        other.Add(2);
        other.Add(3);

        // Act
        list.UnionWith(other);

        // Assert
        list.Should().HaveCount(3);
        list.Head!.Value.Should().Be(1);
        list.Head.Next!.Value.Should().Be(3);
        list.Head.Next.Next!.Value.Should().Be(2);
    }
}
