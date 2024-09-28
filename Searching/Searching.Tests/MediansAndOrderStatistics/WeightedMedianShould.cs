namespace Searching.Tests.MediansAndOrderStatistics;

public class WeightedMedianShould
{
    [Fact]
    public void Find_WhenValuesAndWeightsHaveDifferentLength_ShouldThrowsException()
    {
        // Arrange
        double[] values = { 1, 2, 3 };
        double[] weights = { 1, 2 };

        // Act
        Action act = () => WeightedMedian.Find(values, weights);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Find_WhenValuesIsEmpty_ShouldThrowsException()
    {
        // Arrange
        double[] values = [];
        double[] weights = [];

        // Act
        Action act = () => WeightedMedian.Find(values, weights);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Find_WhenWeightsIsEmpty_ShouldThrowsException()
    {
        // Arrange
        double[] values = { 1, 2, 3 };
        double[] weights = [];

        // Act
        Action act = () => WeightedMedian.Find(values, weights);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Find_WhenValuesHasOneElement_ShouldReturnElement()
    {
        // Arrange
        double[] values = { 1 };
        double[] weights = { 1 };

        // Act
        double result = WeightedMedian.Find(values, weights);

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void Find_WhenDefinedValuesAndWeights_ShouldReturnWeightedMedian()
    {
        // Arrange
        //double[] values = { 1, 2, 3, 4, 5, 6, 7 };
        double[] values = { 0.1, 0.35, 0.05, 0.1, 0.15, 0.05, 0.2 };
        double[] weights = { 0.1, 0.35, 0.05, 0.1, 0.15, 0.05, 0.2 };

        // Act
        double result = WeightedMedian.Find(values, weights);

        // Assert
        result.Should().Be(0.2);
    }
}
