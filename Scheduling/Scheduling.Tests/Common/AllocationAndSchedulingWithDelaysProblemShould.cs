
namespace Scheduling.Tests.Common;

public class AllocationAndSchedulingWithDelaysProblemShould
{
    [Fact]
    public void Solve_WhenGivenArrivalsAndPowerFunction_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 1, 10, 10, 1 };
        var powerFunction = new Func<int, double>(j => Math.Pow(2, j - 1)); // 1, 2, 4, 8

        // Act
        var (Value, ActionsSequence) = AllocationAndSchedulingWithDelaysProblem.Solve(arrivals, powerFunction);

        // Assert
        Value.Should().Be(5);
        ActionsSequence.Should().BeEquivalentTo(new List<int> { 0, 0, 3, 1 });
    }

    [Fact]
    public void Solve_WhenArrivalsAreSmallerThanPowerFunction_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 1, 1, 1, 1 }; // 1 resource arrives at each time step
        var powerFunction = new Func<int, double>(j => 5);

        // Act
        var (Value, ActionsSequence) = AllocationAndSchedulingWithDelaysProblem.Solve(arrivals, powerFunction);

        // Assert
        Value.Should().Be(4); 
        ActionsSequence.Should().BeEquivalentTo(new List<int> { 1, 1, 1, 1 });
    }

    [Fact]
    public void Solve_WhenArrivalsIncrease_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 1, 2, 4, 8 };
        var powerFunction = new Func<int, double>(j => Math.Pow(2, j - 1)); // 1, 2, 4, 8

        // Act
        var (Value, ActionsSequence) = AllocationAndSchedulingWithDelaysProblem.Solve(arrivals, powerFunction);

        // Assert
        Value.Should().Be(8);
        ActionsSequence.Should().BeEquivalentTo(new List<int> { 0, 0, 0, 4 });
    }
}