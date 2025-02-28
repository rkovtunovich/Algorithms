using DataStructures.HashTables.OpenAddressing;

namespace DataStructures.Tests.HashTables.OpenAddressing;

public class SimpleHashSetShould
{
    [Fact]
    public void Add_WhenCalled_ShouldIncreaseCountAndContainElement()
    {
        // Arrange
        var set = new SimpleHashSet<int>
        {
            // Act
            10
        };

        // Assert
        set.Length.Should().Be(1);
        set.Contains(10).Should().BeTrue();
    }

    [Fact]
    public void Contains_WhenElementNotAdded_ShouldReturnFalse()
    {
        // Arrange
        var set = new SimpleHashSet<int>();

        // Act & Assert
        set.Contains(5).Should().BeFalse();
    }

    [Fact]
    public void Remove_WhenElementExists_ShouldDecreaseCountAndNotContainElement()
    {
        // Arrange
        var set = new SimpleHashSet<string>
        {
            "hello",
            "world",
            "test"
        };

        // Act
        set.Remove("world");

        // Assert
        set.Length.Should().Be(2);
        set.Contains("world").Should().BeFalse();
    }

    [Fact]
    public void Add_DuplicateValue_ShouldNotIncreaseCount()
    {
        // Arrange
        var set = new SimpleHashSet<int>
        {
            42,

            // Act
            42
        };

        // Assert
        set.Length.Should().Be(1);
    }

    [Fact]
    public void Resize_WhenLoadFactorExceeded_ShouldPreserveAllElements()
    {
        // Arrange
        var set = new SimpleHashSet<int>();
        // Insert enough elements to force a resize.
        // Given initial capacity is 16 and fill limit is 70%, inserting more than 11 elements will trigger a resize.
        for (int i = 0; i < 15; i++)      
            set.Add(i);
        
        // Act & Assert
        set.Length.Should().Be(15);
        for (int i = 0; i < 15; i++)      
            set.Contains(i).Should().BeTrue();     
    }

    [Fact]
    public void Remove_WhenNonexistentElement_ShouldNotChangeCount()
    {
        // Arrange
        var set = new SimpleHashSet<int>
        {
            1,
            2
        };

        // Act
        set.Remove(3);

        // Assert
        set.Length.Should().Be(2);
    }

    [Fact]
    public void Enumeration_ShouldReturnAllActiveElements()
    {
        // Arrange
        var set = new SimpleHashSet<int>
        {
            1,
            2,
            3
        };

        // Act
        var items = new List<int>();
        foreach (var item in set)       
            items.Add(item);
        
        // Assert
        items.Should().Contain([1, 2, 3]);
        items.Should().HaveCount(3);
    }
}
