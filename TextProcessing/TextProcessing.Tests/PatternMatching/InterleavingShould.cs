using TextProcessing.PatternMatching;

namespace TextProcessing.Tests.PatternMatching;


public class InterleavingShould
{
    [Theory]
    [InlineData("", "", "", true)]
    [InlineData("", "1", "0", true)]  // An empty sequence can be seen as a prefix (length=0) of repeated patterns
    [InlineData("100010101", "101", "00", true)]
    [InlineData("1010", "10", "10", true)]
    [InlineData("111000", "10", "10", false)] // Example: can't form '111000' just with repeated '10'+'10'
    public void IsInterleaving_WithGivenParameters_ReturnsExpectedResult(
        string sequence, string patternX, string patternY, bool expected)
    {
        // Act
        var result = Interleaving.IsInterleaving(sequence, patternX, patternY);

        // Assert
        result.Should().Be(expected,
            $"sequence='{sequence}', x='{patternX}', y='{patternY}' => expected {expected}");
    }

    [Fact]
    public void IsInterleaving_WhenSinglePatternRepeatedManyTimes_ReturnsTrue()
    {
        // Arrange
        // Suppose patternX="01" repeated => possible superposition is "0101"
        // We won't even use patternY because it's empty string
        var sequence = "0101";
        var patternX = "01";
        var patternY = "";

        // Act
        var result = Interleaving.IsInterleaving(sequence, patternX, patternY);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsInterleaving_WhenSequenceNotMatchAnyRepeat_ShouldReturnFalse()
    {
        // Arrange
        // sequence s = '1011'
        // patternX = '10', patternY = '10' => repeated '10' + '10' could produce 
        // e.g. '1010' or '101010' etc. But '1011' has last '1' that doesn't match
        var sequence = "1011";
        var patternX = "10";
        var patternY = "10";

        // Act
        var result = Interleaving.IsInterleaving(sequence, patternX, patternY);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsInterleaving_WhenPatternsIncludeSameBit_ButSequenceIsWrongOrder_ShouldReturnFalse()
    {
        // Arrange
        // sequence = 101
        // x = 1, y = 1 => each is just '1', repeated => we can form '1', '11', '111', etc.
        // But '101' => '1' '0' '1' => the '0' can't come from either repeated '1'.
        var sequence = "101";
        var patternX = "1";
        var patternY = "1";

        // Act
        var result = Interleaving.IsInterleaving(sequence, patternX, patternY);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsInterleaving_WhenBothPatternsSameNontrivial_ShouldReturnTrueIfExactInterleaving()
    {
        // Arrange
        // Let x="10", y="10", and sequence="1010" => 
        // one possible subsequence for x is indices [0,1], for y is [2,3].
        var sequence = "1010";
        var patternX = "10";
        var patternY = "10";

        // Act
        var result = Interleaving.IsInterleaving(sequence, patternX, patternY);

        // Assert
        result.Should().BeTrue("the sequence can be parted into two repeated '10' patterns");
    }
}

