namespace Biology.Tests.Common;

public class ProteinSequenceAligningShould
{
    [Fact]
    public void Align_WhenEmptySequences_ReturnsEmptyAlignment()
    {
        // Arrange & Act
        var (alignment1, alignment2, cost) = ProteinSequenceAligning<char>.Align([], [], 1, 1);

        // Assert
        alignment1.Should().BeEmpty();
        alignment2.Should().BeEmpty();
        cost.Should().Be(0);
    }

    [Fact]
    public void Align_WhenSequencesAreEqual_ReturnsEqualAlignment()
    {
        // Arrange
        var sequence = new List<char> { 'A', 'B', 'C' };

        // Act
        var (alignment1, alignment2, cost) = ProteinSequenceAligning<char>.Align(sequence, sequence, 1, 1);

        // Assert
        alignment1.Should().BeEquivalentTo(sequence);
        alignment2.Should().BeEquivalentTo(sequence);
        cost.Should().Be(0);
    }

    [Fact]
    public void Align_WhenSequencesAreDifferentWithHightGapPenalty_ReturnsCorrectAlignment()
    {
        // Arrange
        var sequence1 = new List<char> { 'A', 'B', 'C' };
        var sequence2 = new List<char> { 'A', 'C', 'D' };

        // Act
        var (alignment1, alignment2, cost) = ProteinSequenceAligning<char>.Align(sequence1, sequence2, 1, 2, '-');

        // Assert
        alignment1.Should().BeEquivalentTo(new List<char> { 'A', 'B', 'C' });
        alignment2.Should().BeEquivalentTo(new List<char> { 'A', 'C', 'D' });
        cost.Should().Be(2);
    }

    [Fact]
    public void Align_WhenSequencesAreDifferentWithHightSymbolPenalty_ReturnsCorrectAlignment()
    {
        // Arrange
        var sequence1 = new List<char> { 'A', 'B', 'C' };
        var sequence2 = new List<char> { 'A', 'C', 'D' };

        // Act
        var (alignment1, alignment2, cost) = ProteinSequenceAligning<char>.Align(sequence1, sequence2, 2, 1, '-');

        // Assert
        alignment1.Should().BeEquivalentTo(new List<char> { 'A', 'B', 'C', '-' });
        alignment2.Should().BeEquivalentTo(new List<char> { 'A', '-', 'C', 'D' });
        cost.Should().Be(2);
    }
    
}
