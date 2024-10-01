namespace DataStructures.Tests.Heaps;

public class TournamentTreeShouldMin
{
    [Fact]
    public void GetExtremum_WhenKeyInAscendingOrder_ShouldReturnMinimum()
    {
        // Arrange
        var leaves = new HeapNode<int, string>[]
        {
            new( 1, "One"),
            new( 2, "Two" ),
            new( 3, "Three"),
            new( 4, "Four" ),
            new( 5, "Five" ),
            new( 6, "Six" ),
            new(7, "Seven"),
            new(8, "Eight" )
        };

        var tree = new TournamentTreeMin<int, string>(new(), leaves);

        // Act
        var minimum = tree.GetExtremum();

        // Assert
        minimum.Should().BeEquivalentTo(new HeapNode<int, string>(1, "One"));
    }

    [Fact]
    public void GetExtremum_WhenKeyInDescendingOrder_ShouldReturnMinimum()
    {
        // Arrange
        var leaves = new HeapNode<int, string>[]
        {
            new( 8, "Eight" ),
            new( 7, "Seven"),
            new( 6, "Six" ),
            new( 5, "Five" ),
            new( 4, "Four" ),
            new( 3, "Three"),
            new( 2, "Two" ),
            new( 1, "One")
        };

        var tree = new TournamentTreeMin<int, string>(new(), leaves);

        // Act
        var minimum = tree.GetExtremum();

        // Assert
        minimum.Should().BeEquivalentTo(new HeapNode<int, string>(1, "One"));
    }

    [Fact]
    public void Update_WhenKeyIsUpdated_ShouldReturnNewMinimum()
    {
        // Arrange
        var leaves = new HeapNode<int, string>[]
        {
            new( 1, "One"),
            new( 2, "Two" ),
            new( 3, "Three"),
            new( 4, "Four" ),
            new( 5, "Five" ),
            new( 6, "Six" ),
            new(7, "Seven"),
            new(8, "Eight" )
        };

        var tree = new TournamentTreeMin<int, string>(new(), leaves);

        // Act
        tree.Update(0, 10, "Ten");
        var minimum = tree.GetExtremum();

        // Assert
        minimum.Should().BeEquivalentTo(new HeapNode<int, string>(2, "Two"));
    }

    [Fact]
    public void Update_WhenKeyIsUpdatedToMinimum_ShouldReturnNewMinimum()
    {
        // Arrange
        var leaves = new HeapNode<int, string>[]
        {
            new( 1, "One"),
            new( 2, "Two" ),
            new( 3, "Three"),
            new( 4, "Four" ),
            new( 5, "Five" ),
            new( 6, "Six" ),
            new(7, "Seven"),
            new(8, "Eight" )
        };

        var tree = new TournamentTreeMin<int, string>(new(), leaves);

        // Act
        tree.Update(0, 0, "Zero");
        var minimum = tree.GetExtremum();

        // Assert
        minimum.Should().BeEquivalentTo(new HeapNode<int, string>(0, "Zero"));
    }
}
