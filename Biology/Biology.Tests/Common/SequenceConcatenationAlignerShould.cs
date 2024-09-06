namespace Biology.Tests.Common;

public class SequenceConcatenationAlignerShould
{
    [Fact]
    public void AlignTargetWithLibrary_WhenZeroCost_ShouldReturnAlignedConcatenationAndCostZero()
    {
        // Arrange
        var A = new List<char> { 'A', 'B', 'C', 'D', 'E' };
        var L = new List<List<char>> { new() { 'A', 'B', 'C' }, new() { 'D', 'E' } };
        double symbolPenalty = 1;
        double gapPenalty = 1;
        char gapSymbol = '-';

        // Act
        var (_, alignedConcatenation, cost) = SequenceConcatenationAligner.AlignTargetWithLibrary(A, L, symbolPenalty, gapPenalty, gapSymbol);

        // Assert
        alignedConcatenation.Should().BeEquivalentTo(A);
        cost.Should().Be(0);
    }

    [Fact]
    public void AlignTargetWithLibrary_WhenNonZeroCost_ShouldReturnAlignedConcatenationAndCostGreaterThanZero()
    {
        // Arrange
        var A = new List<char> { 'A', 'B', 'C', 'D', 'E' };
        var L = new List<List<char>> { new() { 'A', 'B', 'C' }, new() { 'D', 'F' } };
        double symbolPenalty = 1;
        double gapPenalty = 1;
        char gapSymbol = '-';

        // Act
        var (_, alignedConcatenation, cost) = SequenceConcatenationAligner.AlignTargetWithLibrary(A, L, symbolPenalty, gapPenalty, gapSymbol);

        // Assert
        alignedConcatenation.Should().BeEquivalentTo(new List<char> { 'A', 'B', 'C', 'D', 'F' });
        cost.Should().Be(1);
    }
}