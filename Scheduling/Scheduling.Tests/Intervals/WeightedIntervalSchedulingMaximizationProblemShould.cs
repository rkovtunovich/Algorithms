namespace Scheduling.Tests.Intervals;

public class WeightedIntervalSchedulingMaximizationProblemShould
{
    [Fact]
    public void GetOptimalSet_WhenIntervalsAreEmpty()
    {
        // Arrange
        var intervals = new List<WeightedInterval>();

        // Act
        var result = WeightedIntervalSchedulingMaximizationProblem.GetOptimalSet(intervals);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetOptimalSet_WhenAllIntervalsAreNonOverlapping()
    {
        // Arrange
        var intervals = new List<WeightedInterval>
        {
            new(1, 2, 50),
            new(3, 4, 40),
            new(5, 7, 70),
            new(8, 10, 30),
            new(11, 13, 20),
            new(14, 16, 100),
        };

        // Act
        var result = WeightedIntervalSchedulingMaximizationProblem.GetOptimalSet(intervals);

        // Assert
        result.Should().BeEquivalentTo(intervals);
    }

    [Fact]
    public void GetOptimalSet_WhenAllIntervalsOverlapMutually()
    {
        // Arrange
        var intervals = new List<WeightedInterval>
        {
            new(1, 5, 50),
            new(3, 7, 40),
            new(4, 9, 70),
            new(2, 12, 30),
            new(4, 15, 20),
            new(3, 18, 100),
        };

        // Act
        var result = WeightedIntervalSchedulingMaximizationProblem.GetOptimalSet(intervals);

        // Assert
        result.Should().BeEquivalentTo(new List<WeightedInterval> { new(3, 18, 100) });
    }

    [Fact]
    public void GetOptimalSetWithOverlappingIntervalsForSimpleCase()
    {
        // Arrange
        var intervals = new List<WeightedInterval>
        {
            new(1, 3, 1),
            new(2, 5, 3),
            new(4, 6, 1),
        };

        // Act
        var result = WeightedIntervalSchedulingMaximizationProblem.GetOptimalSet(intervals);

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public void GetOptimalSetWithOverlappingIntervals()
    { 
        // 1. [==================] v=2
        // 2.   [=====================] v=4
        // 3.                       [===========] v=4
        // 4.      [==========================================] v=7
        // 5.                                        [==================] v=2
        // 6.                                          [==================] v=1

        // Arrange
        var intervals = new List<WeightedInterval>
        {
            new(1, 4, 2),
            new(2, 6, 4),
            new(5, 7, 4),
            new(3, 10, 7),
            new(8, 11, 2),
            new(9, 12, 1),
        };

        var expected = new List<WeightedInterval>
        {
            new(1, 4, 2),
            new(5, 7, 4),
            new(8, 11, 2),
        };

        // Act
        var result = WeightedIntervalSchedulingMaximizationProblem.GetOptimalSet(intervals);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}

