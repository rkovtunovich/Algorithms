namespace Biology.Tests.Common;

public class RNASecondaryStructureShould
{
    [Theory]
    [InlineData("", 0)]
    [InlineData("A", 0)]
    [InlineData("AU", 0)]
    [InlineData("AUC", 0)]
    [InlineData("AUCG", 0)]
    [InlineData("AUCGA", 0)]
    public void FindMaxBasePairs_WhenNoConditions_ShouldReturnEmptyList(string rna, int expected)
    {
        // Arrange

        // Act
        var basePairs = RNASecondaryStructure.FindMaxBasePairs(rna);

        // Assert
        basePairs.Should().HaveCount(expected);
    }

    [Theory]
    [InlineData("AUCGAU", 1)]
    [InlineData("AUCGAUC", 1)]
    [InlineData("AUCGAUCG", 1)]
    [InlineData("AUCGAUCGA", 2)] // (1, 8), (2, 7)
    [InlineData("AUCGAUCGAU", 3)] // (0, 9), (1, 8), (2, 7)
    public void FindMaxBasePairs_WhenConditionsMet_ShouldReturnBasePairs(string rna, int expected)
    {
        // Arrange

        // Act
        var basePairs = RNASecondaryStructure.FindMaxBasePairs(rna);

        // Assert
        basePairs.Should().HaveCount(expected);
    }
}
