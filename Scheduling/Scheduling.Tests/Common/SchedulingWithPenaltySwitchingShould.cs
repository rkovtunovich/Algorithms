namespace Scheduling.Tests.Common;

public class SchedulingWithPenaltySwitchingShould
{
    [Fact]
    public void Solve_WhenOneSwitching_ShouldReturnOptimalSolution()
    {
        // Arrange
        var stateA = new int[] { 10, 1, 1, 10 };
        var stateB = new int[] { 5, 1, 20, 20 };
        int penaltyA = 1;
        int penaltyB = 1;

        // Act
        var (maxValue, switches) = SchedulingWithPenaltySwitching.Solve(stateA, stateB, penaltyA, penaltyB);

        // Assert
        maxValue.Should().Be(50);
        switches.Should().BeEquivalentTo(new Dictionary<int, (int, int)> { { 2, (1, 2) } } );
    }
}
