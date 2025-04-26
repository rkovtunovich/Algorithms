namespace ResourceOptimization.Tests.Allocation;

public class SchedulingWithDiminishingResourceCapabilitiesShould
{
    [Fact]
    public void Solve_WhenSimpleCaseWithNoRebootNeeded_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 10 };
        var powerFunction = new Func<int, double>(j => 5); // 5

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, 1);

        // Assert
        Value.Should().Be(5);
        RebootsSteps.Should().BeEmpty();
    }

    [Fact]
    public void Solve_WhenGivenArrivalsPowerFunctionAndRebootTime_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 10, 1, 7, 7 };
        var powerFunction = new Func<int, double>(j => Math.Pow(2, 3 - j + 1)); // 8, 4, 2, 1
        var rebootTime = 1;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(19);
        RebootsSteps.Should().BeEquivalentTo(new List<int> { 2 });
    }

    [Fact]
    public void Solve_WhenTheSameArrivalsAndRebootingIsBeneficial_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 5, 5, 5, 5 };
        var powerFunction = new Func<int, double>(j => 5.0 / j);
        var rebootTime = 1;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(12.5);
        RebootsSteps.Should().BeEquivalentTo(new List<int> { 3 });
    }

    [Fact]
    public void Solve_WhenMultipleRebootsNeeded_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 5, 5, 5, 5, 5 };
        var powerFunction = new Func<int, double>(j => 5.0 / j);
        var rebootTime = 1;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(15);
        RebootsSteps.Should().BeEquivalentTo(new List<int> { 2, 4 });
    }

    [Fact]
    public void Solve_WhenRebootingLongerThanTimeHorizon_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 5, 5, 5 };
        var powerFunction = new Func<int, double>(j => 5 - (j - 1));
        var rebootTime = 5;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(12);
        RebootsSteps.Should().BeEmpty();    
    }

    [Fact]
    public void Solve_WhenZeroRebootTime_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 5, 5, 5 };
        var powerFunction = new Func<int, double>(j => 5 - (j - 1));
        var rebootTime = 0;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(15);
        RebootsSteps.Should().BeEquivalentTo(new List<int> { 1, 2, 3 });
    }


    [Fact]
    public void Solve_WhenRebootingWhenPowerIsLow_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 1, 1, 1, 1 };
        var powerFunction = new Func<int, double>(j => 2 - (j - 1));
        var rebootTime = 1;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(3);
        RebootsSteps.Should().BeEquivalentTo(new List<int> { 3 });
    }

    [Fact]
    public void Solve_WhenArrivalsExceedPowerAndPowerIsConstant_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 5, 5, 5, 5 };
        var powerFunction = new Func<int, double>(j => 5);
        var rebootTime = 1;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(20);
        RebootsSteps.Should().BeEmpty();
    }

    [Fact]
    public void Solve_WhenZeroArrivals_ShouldReturnZeroValue()
    {
        // Arrange
        var arrivals = new int[] { 0, 0, 0, 0 };
        var powerFunction = new Func<int, double>(j => 5);
        var rebootTime = 1;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(0);
        RebootsSteps.Should().BeEmpty();
    }

    [Fact]
    public void Solve_WhenRebootBeforeHighArrivals_ShouldReturnOptimalValue()
    {
        // Arrange
        var arrivals = new int[] { 0, 0, 5 };
        var powerFunction = new Func<int, double>(j => 5 - (j - 1));
        var rebootTime = 1;

        // Act
        var (Value, RebootsSteps) = SchedulingWithDiminishingResourceCapabilities.Solve(arrivals, powerFunction, rebootTime);

        // Assert
        Value.Should().Be(5);
        RebootsSteps.Should().BeEquivalentTo(new List<int> { 2 });
    }
}
