using Searching.Common;

namespace Searching.Tests.Common;
public class LinearSubsequenceSearchShould
{
    [Fact]
    public void Search_SubsequencePresentsInSequence_FirstIndexOfSubsequence()
    {
        // arrange
        var array = new int[] { 1, 2, 3, 8, 4, 5, 6 };
        var subsequence = new int[] { 3, 4, 5 };

        // act
        var indexes = LinearSubsequenceSearch<int>.Search(array, subsequence);

        // assert
        indexes.Should().NotBeNullOrEmpty();
        indexes.Length.Should().Be(subsequence.Length);
        indexes[0].Should().Be(2);
        indexes[1].Should().Be(4);
        indexes[2].Should().Be(5);
    } 
}
