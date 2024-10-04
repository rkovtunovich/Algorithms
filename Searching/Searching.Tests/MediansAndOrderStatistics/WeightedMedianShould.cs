namespace Searching.Tests.MediansAndOrderStatistics;

public class WeightedMedianShould
{

    [Fact]
    public void Find_WhenValuesIsEmpty_ShouldThrowException()
    {
        // Arrange
        var weightedValues = Array.Empty<WeightedValue>();

        // Act
        Action act = () => WeightedMedian.Find(weightedValues);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Find_WhenValuesHasOneElement_ShouldReturnElement()
    {
        // Arrange
        var weightedValues = new WeightedValue[] { new(1, 1) };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Should().Be(new WeightedValue(1, 1));
    }

    [Fact]
    public void Find_WhenAllWeightsAreEqual_ShouldReturnMedian()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 1),
            new(2, 1),
            new(3, 1),
            new(4, 1),
            new(5, 1)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Value.Should().Be(3);
    }

    [Fact]
    public void Find_WhenOneDominantWeight_ShouldReturnDominantValue()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 0.1),
            new(2, 0.1),
            new(3, 0.1),
            new(4, 0.1),
            new(5, 0.6)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Should().Be(new WeightedValue(5, 0.6));
    }

    [Fact]
    public void Find_WhenDefinedDifferentWeights_ShouldReturnWeightedMedian()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(0.1, 0.1),
            new(0.35, 0.35),
            new(0.05, 0.05),
            new(0.1, 0.1),
            new(0.15, 0.15),
            new(0.05, 0.05),
            new(0.2, 0.2)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Should().Be(new WeightedValue(0.2, 0.2));
    }

    [Fact]
    public void Find_WhenSumLessThenOne_ShouldReturnSmallestValue()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 0.1),
            new(2, 0.1),
            new(3, 0.1),
            new(4, 0.1),
            new(5, 0.1)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Value.Should().Be(3);
    }

    [Fact]
    public void Find_WhenElementsWithZeroWeight_ShouldReturnMedian()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 0.1),
            new(2, 0),
            new(3, 0.1),
            new(4, 0),
            new(5, 0.1)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Value.Should().Be(3);
    }

    [Fact]
    public void Find_WhenUniformDistribution_ShouldReturnMedian()
    {
        // Arrange
        var weightedValues = new WeightedValue[10];
        for (int i = 1; i <= 10; i++)
        {
            weightedValues[i - 1] = new WeightedValue(i, 1);
        }

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Value.Should().Be(5);
    }

    [Fact]
    public void Find_WhenAllWeightsAreVerySmall_ShouldReturnMedian()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 1e-10),
            new(2, 1e-10),
            new(3, 1e-10),
            new(4, 1e-10),
            new(5, 1e-10),
            new(6, 1e-10),
            new(7, 1e-10),
            new(8, 1e-10),
            new(9, 0.999999991)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Value.Should().Be(9);
    }

    [Fact]
    public void Find_WhenNegativeWeights_ShouldThrowException()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 1),
            new(2, -1),
            new(3, 1)
        };

        // Act
        Action act = () => WeightedMedian.Find(weightedValues);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Find_WhenZeroTotalWeight_ShouldThrowException()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 0),
            new(2, 0),
            new(3, 0)
        };

        // Act
        Action act = () => WeightedMedian.Find(weightedValues);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Find_WhenWeightsSumCloseTo0_5_ShouldReturnMedian()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 0.25),
            new(2, 0.25),
            new(3, 0.25),
            new(4, 0.25)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Value.Should().Be(2);
    }

    [Fact]
    public void Find_WhenAscendingOrder_ShouldReturnMedian()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(1, 0.05),
            new(2, 0.1),
            new(3, 0.15),
            new(4, 0.2),
            new(5, 0.25),
            new(6, 0.25)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Value.Should().Be(4);
    }

    [Fact]
    public void Find_WhenDescendingOrder_ShouldReturnMedian()
    {
        // Arrange
        var weightedValues = new WeightedValue[]
        {
            new(6, 0.25),
            new(5, 0.25),
            new(4, 0.2),
            new(3, 0.15),
            new(2, 0.1),
            new(1, 0.05)
        };

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        result.Value.Should().Be(4);
    }

    [Fact]
    public void Find_WhenRandomizedWeightsAndValues_ShouldReturnWeightedMedian()
    {
        // Arrange
        var random = new Random();
        var weightedValues = new WeightedValue[20];
        double totalWeight = 0;

        for (int i = 0; i < 20; i++)
        {
            double value = random.NextDouble() * 100; // Random value between 0 and 100
            double weight = random.NextDouble();      // Random weight between 0 and 1
            weightedValues[i] = new WeightedValue(value, weight);
            totalWeight += weight;
        }

        // Normalize weights
        for (int i = 0; i < 20; i++)
        {
            weightedValues[i].Weight /= totalWeight;
        }

        // Act
        var result = WeightedMedian.Find(weightedValues);

        // Assert
        var indexOfMedian = Array.FindIndex(weightedValues, x => x.Value == result.Value);

        double totalWeightLeft = 0;
        for (int i = 0; i < indexOfMedian; i++)
        {
            totalWeightLeft += weightedValues[i].Weight;
        }

        double totalWeightRight = 0;
        for (int i = indexOfMedian + 1; i < 20; i++)
        {
            totalWeightRight += weightedValues[i].Weight;
        }

        totalWeightLeft.Should().BeLessThanOrEqualTo(0.5);
        totalWeightRight.Should().BeLessThan(0.5);
    }
}
