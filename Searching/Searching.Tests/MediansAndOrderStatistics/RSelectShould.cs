namespace Searching.Tests.MediansAndOrderStatistics;

public class RSelectShould
{
    [Fact]
    public void Find_WhenArrayIsEmpty_ShouldThrowsException()
    {
        // Arrange
        int[] array = [];
        int orderStatistics = 0;

        // Act
        Action act = () => RSelect<int>.Find(array, orderStatistics);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Find_WhenOrderStatisticsIsNegative_ShouldThrowsException()
    {
        // Arrange
        int[] array = { 1, 2, 3 };
        int orderStatistics = -1;

        // Act
        Action act = () => RSelect<int>.Find(array, orderStatistics);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Find_WhenOrderStatisticsIsGreaterThanArrayLength_ShouldThrowsException()
    {
        // Arrange
        int[] array = { 1, 2, 3 };
        int orderStatistics = 3;

        // Act
        Action act = () => RSelect<int>.Find(array, orderStatistics);

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Find_WhenOrderStatisticsIsZero_ShouldReturnSmallestElement()
    {
        // Arrange
        int[] array = { 3, 2, 1 };
        int orderStatistics = 0;

        // Act
        (int index, int element) = RSelect<int>.Find(array, orderStatistics);

        // Assert
        index.Should().Be(0);
        element.Should().Be(1);
    }

    [Fact]
    public void Find_WhenOrderStatisticsIsMiddle_ShouldReturnMiddleElement()
    {
        // Arrange
        int[] array = { 3, 2, 1 };
        int orderStatistics = 1;

        // Act
        (int index, int element) = RSelect<int>.Find(array, orderStatistics);

        // Assert
        index.Should().Be(1);
        element.Should().Be(2);
    }

    [Fact]
    public void Find_WhenOrderStatisticsIsLast_ShouldReturnLargestElement()
    {
        // Arrange
        int[] array = { 3, 2, 1 };
        int orderStatistics = 2;

        // Act
        (int index, int element) = RSelect<int>.Find(array, orderStatistics);

        // Assert
        index.Should().Be(2);
        element.Should().Be(3);
    }
}
