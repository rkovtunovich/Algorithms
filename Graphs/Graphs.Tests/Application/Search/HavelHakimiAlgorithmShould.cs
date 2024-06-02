using DataStructures.Lists;
using Graphs.Application.Search;

namespace Graphs.Tests.Application.Search;

public class HavelHakimiAlgorithmShould
{
    [Fact]
    public void ReturnTrue_GivenGraphicalSequence()
    {
        // Arrange
        var sequence = new SequentialList<int> { 3, 3, 2, 2, 1, 1 };

        // Act
        var result = HavelHakimiAlgorithm.IsGraphical(sequence);

        // Assert
        result.Should().BeTrue(); 
    }

    [Fact]
    public void ReturnFalse_GivenNonGraphicalSequence()
    {
        // Arrange
        var sequence = new SequentialList<int> { 3, 3, 2, 2, 1, 0 };

        // Act
        var result = HavelHakimiAlgorithm.IsGraphical(sequence);

        // Assert
        result.Should().BeFalse();
    }
}
