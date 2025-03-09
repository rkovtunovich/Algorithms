namespace Searching.Tests.Common;

public class LongestIncreasingSubsequenceShould
{
    [Fact]
    public void ComputeLIS_WhenEmptyArray_ReturnsEmpty()
    {
        // Arrange
        var arr = Array.Empty<int>();

        // Act
        var result = LongestIncreasingSubsequence.ComputeLIS(arr);

        // Assert
        result.Should().BeEmpty("the LIS of an empty array is an empty sequence.");
    }

    [Fact]
    public void ComputeLIS_WhenSingleElement_ReturnsElementItself()
    {
        // Arrange
        var arr = new int[] { 42 };

        // Act
        var result = LongestIncreasingSubsequence.ComputeLIS(arr);

        // Assert
        result.Should().ContainSingle()
              .And.ContainInOrder(new List<int> { 42 });
    }

    [Fact]
    public void ComputeLIS_WhenAllIncreasing_ReturnsEntireArray()
    {
        // Arrange
        var arr = new int[] { 1, 2, 3, 4, 5 };

        // Act
        var result = LongestIncreasingSubsequence.ComputeLIS(arr);

        // Assert
        // The entire array is strictly increasing, so the LIS is the array itself
        result.Should().BeEquivalentTo(arr, options => options.WithStrictOrdering());
    }

    [Fact]
    public void ComputeLIS_WhenAllDecreasing_ReturnsSingleElement()
    {
        // Arrange
        var arr = new int[] { 10, 9, 5, 2, 1 };

        // Act
        var result = LongestIncreasingSubsequence.ComputeLIS(arr);

        // Assert
        // In a strictly decreasing array, the LIS is any single element
        result.Should().ContainSingle("the best we can do is pick one element");

        // Any of these values is acceptable
        result[0].Should().BeOneOf(10, 9, 5, 2, 1);
    }

    [Fact]
    public void ComputeLIS_WhenContainsDuplicates_SkipsDuplicatesForStrictlyIncreasing()
    {
        // Arrange
        var arr = new int[] { 2, 2, 2, 3, 3, 4 };

        // Act
        var result = LongestIncreasingSubsequence.ComputeLIS(arr);

        // Assert
        // The subsequence must strictly increase, so duplicates can't be part of the same subsequence
        // A valid LIS might be [2, 3, 4]
        result.Should().BeEquivalentTo([2, 3, 4], options => options.WithStrictOrdering());
    }

    [Fact]
    public void ComputeLIS_WhenMixedNumbers_ReturnsCorrectSubsequence()
    {
        // Arrange
        var arr = new int[] { 10, 1, 2, 11, 3, 4, 12 };

        // Act
        var result = LongestIncreasingSubsequence.ComputeLIS(arr);

        // Assert
        // Possible LIS includes [1, 2, 3, 4, 12] of length 5
        // Or [1, 2, 11, 12] also length 4, but the first is longer (5).
        // We check for the typical [1,2,3,4,12].
        result.Should().HaveCount(5);
        result.Should().StartWith(1);
        result.Should().HaveElementAt(result.Count - 1, 12);
        // Strictly increasing check
        for (int i = 0; i < result.Count - 1; i++)       
            result[i].Should().BeLessThan(result[i + 1], "subsequence must be strictly increasing");
        
    }
}
