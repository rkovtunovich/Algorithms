namespace Spacing.Tests;

public class TwoDimensionShould
{
    [Fact]
    public void GetClosestPoints_WhenGivenPoints_ReturnsClosestPoints()
    {
        // Arrange
        var points = new Point2D[]
        {
            new(2, 3),
            new(12, 30),
            new(40, 50),
            new(5, 1),
            new(12, 10),
            new(3, 4)
        };

        // Act
        var (point1, point2, distance) = TwoDimension.GetClosestPoints(points);

        // Assert
        point1.Should().BeEquivalentTo(new Point2D(2, 3));
        point2.Should().BeEquivalentTo(new Point2D(3, 4));
        distance.Should().BeGreaterThan(1.4);
        distance.Should().BeLessThan(1.5);

    }

    [Fact]
    public void GetClosestPoints_WhenGivenPointsAndShowSteps_ReturnsClosestPoints()
    {
        // Arrange
        var points = new Point2D[]
        {
            new(2, 3),
            new(12, 30),
            new(40, 50),
            new(5, 1),
            new(12, 10),
            new(3, 4)
        };

        // Act
        var (point1, point2, distance) = TwoDimension.GetClosestPoints(points, true);

        // Assert
        point1.Should().BeEquivalentTo(new Point2D(2, 3));
        point2.Should().BeEquivalentTo(new Point2D(3, 4));
        distance.Should().BeGreaterThan(1.4);
        distance.Should().BeLessThan(1.5);
    }

    [Fact]
    public void GetClosestPoints_WhenGivenLessThanTwoPoints_ThrowsArgumentException()
    {
        // Arrange
        var points = new Point2D[]
        {
            new(2, 3)
        };

        // Act
        Action act = () => TwoDimension.GetClosestPoints(points);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("At least two points are required.");

    }
}

