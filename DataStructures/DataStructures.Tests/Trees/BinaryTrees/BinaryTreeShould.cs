namespace DataStructures.Tests.Trees.BinaryTrees;

public class BinaryTreeShould
{
    [Fact]
    public void Root_WhenTreeIsEmpty_ShouldBeNull()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();

        // Act
        var root = tree.Root;

        // Assert
        root.Should().BeNull();
    }

    [Fact]
    public void Root_WhenTreeIsNotEmpty_ShouldReturnRootNode()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(1, 1);
        tree.Insert(2, 2);

        // Act
        var root = tree.Root;

        // Assert
        root.Should().NotBeNull();
        root.Key.Should().Be(1);
    }

    [Fact]
    public void Insert_WhenTreeIsEmpty_ShouldInsertRootNode()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();

        // Act
        tree.Insert(1, 1);

        // Assert
        tree.Root.Should().NotBeNull();
        tree.Root.Key.Should().Be(1);
    }

    [Fact]
    public void Insert_WhenTreeIsNotEmpty_ShouldInsertNode()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(1, 1);

        // Act
        tree.Insert(2, 2);

        // Assert
        tree.Root.RightChild.Should().NotBeNull();
        tree.Root.RightChild.Key.Should().Be(2);
    }

    [Fact]
    public void Remove_WhenTreeIsEmpty_ShouldDoNothing()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();

        // Act
        tree.Remove(1);

        // Assert
        tree.Root.Should().BeNull();
    }

    [Fact]
    public void Remove_WhenNodeIsRoot_ShouldRemoveRootNode()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(1, 1);

        // Act
        tree.Remove(1);

        // Assert
        tree.Root.Should().BeNull();
    }

    [Fact]
    public void Remove_WhenNodeIsLeaf_ShouldRemoveLeafNode()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(1, 1);
        tree.Insert(2, 2);

        // Act
        tree.Remove(2);

        // Assert
        tree.Root.RightChild.Should().BeNull();
    }

    [Fact]
    public void Remove_WhenNodeHasOneRightChild_ShouldRemoveNodeAndPromoteChild()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(1, 1);
        tree.Insert(2, 2);

        // Act
        tree.Remove(1);

        // Assert
        tree.Root.Key.Should().Be(2);
    }

    [Fact]
    public void Remove_WhenNodeHasOneLeftChild_ShouldRemoveNodeAndPromoteChild()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(2, 2);
        tree.Insert(1, 1);

        // Act
        tree.Remove(2);

        // Assert
        tree.Root.Key.Should().Be(1);
    }

    [Fact]
    public void Remove_WhenNodeHasTwoChildren_ShouldRemoveNodeAndPromoteInOrderSuccessor()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(2, 2);
        tree.Insert(1, 1);
        tree.Insert(3, 3);

        // Act
        tree.Remove(2);

        // Assert
        tree.Root.Key.Should().Be(1);
        tree.Root.RightChild.Key.Should().Be(3);
    }

    [Fact]
    public void Remove_WhenNodeHasTwoChildrenAndSuccessorHasRightChild_ShouldRemoveNodeAndPromoteInOrderSuccessor()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(3, 3);
        tree.Insert(2, 2);
        tree.Insert(1, 1);
        tree.Insert(4, 4);

        // Act
        tree.Remove(3);

        // Assert
        tree.Root.Key.Should().Be(2);
        tree.Root.LeftChild.Key.Should().Be(1);
        tree.Root.RightChild.Key.Should().Be(4);
    }

    [Fact]
    public void Remove_WhenNodeHasTwoChildrenAndSuccessorIsRightChild_ShouldRemoveNodeAndPromoteInOrderSuccessor()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(3, 3);
        tree.Insert(1, 1);
        tree.Insert(4, 4);
        tree.Insert(2, 2);

        // Act
        tree.Remove(3);

        // Assert
        tree.Root.Key.Should().Be(2);
        tree.Root.LeftChild.Key.Should().Be(1);
        tree.Root.RightChild.Key.Should().Be(4);
    }

    [Fact]
    public void Clear_WhenTreeIsNotEmpty_ShouldRemoveAllNodes()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(1, 1);
        tree.Insert(2, 2);

        tree.Clear();

        // Assert
        tree.Root.Should().BeNull();
    }

    [Fact]
    public void TraverseInOrder_WhenTreeIsNotEmpty_ShouldTraverseInOrder()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(2, 0);
        tree.Insert(1, 0);
        tree.Insert(3, 0);

        var capturedKeys = new List<int>();

        // Act
        tree.TraverseInOrder(node => capturedKeys.Add(node.Key));

        // Assert
        capturedKeys.Should().BeEquivalentTo([1, 2, 3]);
    }

    [Fact]
    public void TraversePreOrder_WhenTreeIsNotEmpty_ShouldTraversePreOrder()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(2, 0);
        tree.Insert(1, 0);
        tree.Insert(3, 0);
        var capturedKeys = new List<int>();
        
        // Act
        tree.TraversePreOrder(node => capturedKeys.Add(node.Key));
        
        // Assert
        capturedKeys.Should().BeEquivalentTo([2, 1, 3]);
    }

    [Fact]
    public void TraversePostOrder_WhenTreeIsNotEmpty_ShouldTraversePostOrder()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(2, 0);
        tree.Insert(1, 0);
        tree.Insert(3, 0);
        var capturedKeys = new List<int>();
        
        // Act
        tree.TraversePostOrder(node => capturedKeys.Add(node.Key));
        
        // Assert
        capturedKeys.Should().BeEquivalentTo([1, 3, 2]);
    }

    [Fact]
    public void TraverseInOrderMorris_WhenTreeIsNotEmpty_ShouldTraverseInOrder()
    {
        // Arrange
        var tree = new BinaryTree<int, int>();
        tree.Insert(2, 2);
        tree.Insert(1, 1);
        tree.Insert(3, 3);
        tree.Insert(4, 4);
        tree.Insert(5, 5);
        tree.Insert(6, 6);

        var capturedKeys = new List<int>();
        
        // Act
        tree.TraverseInOrderMorris(node => capturedKeys.Add(node.Key));
        
        // Assert
        capturedKeys.Should().BeEquivalentTo([1, 2, 3, 4, 5, 6]);
    }
}
