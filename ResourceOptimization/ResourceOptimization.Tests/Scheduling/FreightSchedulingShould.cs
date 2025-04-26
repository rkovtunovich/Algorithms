using ResourceOptimization.Scheduling;

namespace ResourceOptimization.Tests.Scheduling;

public class FreightSchedulingShould
{
    [Fact]
    public void MinShippingCost_WhenSupplies_ShouldReturnOptimalSchedule()
    {
        // Arrange
        int[] supplies = { 11, 9, 9, 12, 12, 12, 12, 9, 9, 11 }; // sum = 106, optimal = 98
        int ratePerUnit = 1;
        int flatCost = 10;

        // Act
        var (minCost, schedule) = FreightScheduling.MinShippingCost(supplies, ratePerUnit, flatCost);

        // Assert
        minCost.Should().Be(98);
        schedule.Should().BeEquivalentTo(new List<string> { "A", "A", "A", "B", "B", "B", "B", "A", "A", "A" });
    }

    [Fact]
    public void MinShippingCost_WhenTwoSwitches_ShouldReturnOptimalSchedule()
    {
        // Arrange
        int[] supplies = { 9, 9, 12, 12, 12, 3, 12, 12, 12, 9, 9, 8 };
        int ratePerUnit = 1;
        int flatCost = 10;
        int minTerm = 3;

        // Act
        var (minCost, schedule) = FreightScheduling.MinShippingCost(supplies, ratePerUnit, flatCost, minTerm);

        // Assert
        minCost.Should().Be(107);
        schedule.Should().BeEquivalentTo(new List<string> { "A", "A", "B", "B", "B", "A", "B", "B", "B", "A", "A", "A" });

    }

    [Fact]
    public void MinShippingCost_WhenLongerThatMinimumTermExists_ShouldReturnOptimalSchedule()
    {
        // Arrange
        int[] supplies = { 1, 1, 10, 10, 10, 10, 10, 1 };
        int ratePerUnit = 1;
        int flatCost = 5;
        int minTerm = 3;

        // Act
        var (minCost, schedule) = FreightScheduling.MinShippingCost(supplies, ratePerUnit, flatCost, minTerm);

        // Assert
        minCost.Should().Be(28);
        schedule.Should().BeEquivalentTo(new List<string> { "A", "A", "B", "B", "B", "B", "B", "A" });
    }

    [Fact]
    public void MinShippingCost_WhenNoSupplies_ShouldReturnZeroCost()
    {
        // Arrange
        int[] supplies = [];
        int ratePerUnit = 1;
        int flatCost = 5;
        // Act
        var (minCost, schedule) = FreightScheduling.MinShippingCost(supplies, ratePerUnit, flatCost);
        // Assert
        minCost.Should().Be(0);
        schedule.Should().BeEmpty();
    }

    [Fact]
    public void MinShippingCost_WhenMinimumTermIsLargerThanSupplies_ShouldReturnCostPerUnit()
    {
        // Arrange
        int[] supplies = { 1, 1, 1 };
        int ratePerUnit = 1;
        int flatCost = 5;
        int minTerm = 4;
        // Act
        var (minCost, schedule) = FreightScheduling.MinShippingCost(supplies, ratePerUnit, flatCost, minTerm);
        // Assert
        minCost.Should().Be(3);
        schedule.Should().BeEquivalentTo(new List<string> { "A", "A", "A" });
    }

    [Fact]
    public void MinShippingCost_WhenMinimumTermIsEqualToSupplies_ShouldReturnFlatCost()
    {
        // Arrange
        int[] supplies = { 1, 1, 1, 1 };
        int ratePerUnit = 2;
        int flatCost = 1;
        int minTerm = 4;
        // Act
        var (minCost, schedule) = FreightScheduling.MinShippingCost(supplies, ratePerUnit, flatCost, minTerm);
        // Assert
        minCost.Should().Be(4);
        schedule.Should().BeEquivalentTo(new List<string> { "B", "B", "B", "B" });
    }
}