
namespace Searching.Tests.Common;

public class MinMaxSearchShould
{
    // Test cases for FindMinMax method

    // Test case 1:
    // An array with a single element should return that element as both the minimum and maximum.
    [Fact]
    public void FindMinMax_WhenSingleElement_ShouldReturnElementAsMinMax()
    {
        // Arrange
        int[] array = { 5 };

        // Act
        var (min, max) = MinMaxSearch.FindMinMax(array);

        // Assert
        min.Should().Be(5);
        max.Should().Be(5);
    }

    // Test case 2:
    // An array with two elements should return the smaller element as the minimum and the larger element as the maximum.
    [Fact]
    public void FindMinMax_WhenTwoElements_ShouldReturnMinAsSmallerAndMaxAsLargerElement()
    {
        // Arrange
        int[] array = { 5, 3 };

        // Act
        var (min, max) = MinMaxSearch.FindMinMax(array);

        // Assert
        min.Should().Be(3);
        max.Should().Be(5);
    }

    // Test case 3:
    // An array with multiple elements should return the minimum and maximum elements correctly.
    [Fact]
    public void FindMinMax_WhenMultipleElements_ShouldReturnMinAndMaxCorrectly()
    {
        // Arrange
        int[] array = { 5, 3, 9, 1, 7, 2, 8, 4, 6 };

        // Act
        var (min, max) = MinMaxSearch.FindMinMax(array);

        // Assert
        min.Should().Be(1);
        max.Should().Be(9);
    }
}
