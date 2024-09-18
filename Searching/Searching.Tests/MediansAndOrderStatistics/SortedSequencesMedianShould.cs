namespace Searching.Tests.MediansAndOrderStatistics;

public class SortedSequencesMedianShould
{
    [Fact]
    public void FindMedian_WhenSequencesAreEmpty_ShouldReturnZero()
    {
        // Arrange
        var sequence1 = Array.Empty<int>();
        var sequence2 = Array.Empty<int>();

        // Act
        var median = SortedSequencesMedian.FindMedian(sequence1, sequence2);

        // Assert
        median.Should().Be(0);
    }

    [Fact]
    public void FindMedian_WhenSequencesHaveOneElement_ShouldReturnElement()
    {
        // Arrange
        var sequence1 = new[] { 1 };
        var sequence2 = new[] { 2 };

        // Act
        var median = SortedSequencesMedian.FindMedian(sequence1, sequence2);

        // Assert
        median.Should().Be(1.5);
    }

    [Fact]
    public void FindMedian_WhenSequencesHaveTwoElements_ShouldReturnMedian()
    {
        // Arrange
        var sequence1 = new[] { 1, 3 };
        var sequence2 = new[] { 2, 4 };

        // Act
        var median = SortedSequencesMedian.FindMedian(sequence1, sequence2);

        // Assert
        median.Should().Be(2.5);
    }

    [Fact]
    public void FindMedian_WhenSequencesHaveThreeElements_ShouldReturnMedian()
    {
        // Arrange
        var sequence1 = new[] { 1, 3, 5 };
        var sequence2 = new[] { 2, 4, 6 };

        // Act
        var median = SortedSequencesMedian.FindMedian(sequence1, sequence2);

        // Assert
        median.Should().Be(3.5);
    }

    [Fact]
    public void FindMedian_WhenSequencesHaveDifferentLengths_ShouldReturnMedian()
    {
        // Arrange
        var sequence1 = new[] { 1, 3, 5, 7, 9 };
        var sequence2 = new[] { 2, 4, 6, 8 };

        // Act
        var median = SortedSequencesMedian.FindMedian(sequence1, sequence2);

        // Assert
        median.Should().Be(5);
    }

    [Fact]
    public void FindMedian_WhenSequencesHaveLength8_ShouldReturnMedian()
    {
        // Arrange
        var sequence1 = new[] { 10, 23, 73, 77, 82, 87, 96, 97 };
        var sequence2 = new[] { 18, 34, 35, 40, 61, 63, 65, 84 };

        // Act
        var median = SortedSequencesMedian.FindMedian(sequence1, sequence2);

        // Assert
        median.Should().Be(64);
    }
}
