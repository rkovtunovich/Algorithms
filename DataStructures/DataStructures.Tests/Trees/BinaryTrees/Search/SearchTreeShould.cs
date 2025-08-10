using DataStructures.Trees.BinaryTrees.Search;

namespace DataStructures.Tests.Trees.BinaryTrees.Search;

public class SearchTreeShould
{
    private static SearchTree<int, int> CreateTree()
    {
        var tree = new SearchTree<int, int>();
        // Constructing the following tree:
        //       5
        //      / \
        //     3   7
        //    / \ / 
        //   2  4 6  

        tree.Insert(5, 5);
        tree.Insert(3, 3);
        tree.Insert(7, 7);
        tree.Insert(2, 2);
        tree.Insert(4, 4);
        tree.Insert(6, 6);
        return tree;
    }

    [Fact]
    public void Search_WhenKeyExists_ShouldReturnNode()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var node = tree.Search(4);

        // Assert
        node.Should().NotBeNull();
        node!.Key.Should().Be(4);
        node.Value.Should().Be(4);
    }

    [Fact]
    public void Search_WhenKeyDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var node = tree.Search(10);

        // Assert
        node.Should().BeNull();
    }

    [Fact]
    public void Minimum_WhenTreeIsEmpty_ShouldBeNull()
    {
        // Arrange
        var tree = new SearchTree<int, int>();

        // Act
        var min = tree.Minimum;

        // Assert
        min.Should().BeNull();
    }

    [Fact]
    public void Minimum_WhenTreeHasNodes_ShouldReturnMinimumNode()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var min = tree.Minimum;

        // Assert
        min.Should().NotBeNull();
        min!.Key.Should().Be(2);
    }

    [Fact]
    public void Maximum_WhenTreeIsEmpty_ShouldBeNull()
    {
        // Arrange
        var tree = new SearchTree<int, int>();

        // Act
        var max = tree.Maximum;

        // Assert
        max.Should().BeNull();
    }

    [Fact]
    public void Maximum_WhenTreeHasNodes_ShouldReturnMaximumNode()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var max = tree.Maximum;

        // Assert
        max.Should().NotBeNull();
        max!.Key.Should().Be(7);
    }

    [Fact]
    public void GetPredecessor_WhenKeyDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var predecessor = tree.GetPredecessor(10);

        // Assert
        predecessor.Should().BeNull();
    }

    [Fact]
    public void GetPredecessor_WhenNodeHasLeftChild_ShouldReturnMaxOfLeftSubtree()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var predecessor = tree.GetPredecessor(5);

        // Assert
        predecessor.Should().NotBeNull();
        predecessor!.Key.Should().Be(4);
    }

    [Fact]
    public void GetPredecessor_WhenNodeIsRightChild_ShouldReturnParent()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var predecessor = tree.GetPredecessor(4);

        // Assert
        predecessor.Should().NotBeNull();
        predecessor!.Key.Should().Be(3);
    }

    [Fact]
    public void GetPredecessor_WhenNodeIsMinimum_ShouldReturnNull()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var predecessor = tree.GetPredecessor(2);

        // Assert
        predecessor.Should().BeNull();
    }

    [Fact]
    public void GetSuccessor_WhenKeyDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var successor = tree.GetSuccessor(10);

        // Assert
        successor.Should().BeNull();
    }

    [Fact]
    public void GetSuccessor_WhenNodeHasRightChild_ShouldReturnMinOfRightSubtree()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var successor = tree.GetSuccessor(5);

        // Assert
        successor.Should().NotBeNull();
        successor!.Key.Should().Be(6);
    }

    [Fact]
    public void GetSuccessor_WhenNodeIsLeftChild_ShouldReturnParent()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var successor = tree.GetSuccessor(2);

        // Assert
        successor.Should().NotBeNull();
        successor!.Key.Should().Be(3);
    }

    [Fact]
    public void GetSuccessor_WhenNodeIsRightChild_ShouldReturnGrandparent()
    {
        // Arrange
        var tree = CreateTree();

        // Act
        var successor = tree.GetSuccessor(4);

        // Assert
        successor.Should().NotBeNull();
        successor!.Key.Should().Be(5);
    }

    [Fact]
    public void GetSuccessor_WhenNoRightChild_ShouldReturnSuccessor()
    {
        // Arrange
       //     7
       //   /   \
       //  3     8
       // / \     \
       //2   5     9
       //   /
       //  4
        var tree = new SearchTree<int, int>();
        tree.Insert(7);
        tree.Insert(2);
        tree.Insert(3);
        tree.Insert(4);
        tree.Insert(5);
        tree.Insert(8);
        tree.Insert(9);

        // Act
        var successor = tree.GetSuccessor(5);

        // Assert
        successor.Should().NotBeNull();
        successor!.Key.Should().Be(7);
    }
}
