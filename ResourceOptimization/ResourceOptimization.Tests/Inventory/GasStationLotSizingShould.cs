namespace ResourceOptimization.Tests.Inventory;

public class GasStationLotSizingShould
{
    [Fact]
    public void ReturnEmptyPlan_WhenNoDays()
    {
        // Arrange
        // An empty demand array means no days to plan for
        var demand = Array.Empty<int>();

        // Act
        var result = GasStationLotSizing.Solve(demand, 5, 2, 1, 10);

        // Assert
        result.TotalCost.Should().Be(0);
        result.Orders.Should().BeEmpty();
    }

    [Fact]
    public void HandleSingleDayDemand()
    {
        // Arrange
        int[] demand = [7];

        // Act
        var plan = GasStationLotSizing.Solve(demand, 10, 3, 1, 10);

        // Assert
        plan.TotalCost.Should().Be(10 + 7 * 3);
        plan.Orders.Should().ContainSingle().Which.Day.Should().Be(1);
        plan.Orders.Single().Quantity.Should().Be(7);
    }

    [Fact]
    public void RespectCapacityConstraint()
    {
        // Arrange
        int[] demand = [2, 2, 2];

        // Act
        // capacity only allows ordering one day's demand at a time
        var plan = GasStationLotSizing.Solve(demand, 5, 1, 0.5, 2);

        // Assert
        plan.Orders.Should().HaveCount(3);
        plan.Orders.Should().OnlyContain(o => o.Quantity == 2);
    }

    [Fact]
    public void SkipZeroDemands()
    {
        // Arrange
        int[] demand = [0, 0, 3];

        // Act
        var plan = GasStationLotSizing.Solve(demand, 4, 2, 1, 5);

        // Assert
        plan.Orders.Should().ContainSingle();
        var order = plan.Orders[0];
        order.Day.Should().Be(3);
        order.Quantity.Should().Be(3);
    }

    [Fact]
    public void HandleDemandsEqualToCapacity()
    {
        // Arrange
        int[] demand = [5, 5];

        // Act
        var plan = GasStationLotSizing.Solve(demand, 1, 1, 1, 5);

        // Assert
        plan.Orders.Should().HaveCount(2);
        plan.Orders[0].Quantity.Should().Be(5);
        plan.Orders[1].Quantity.Should().Be(5);
    }

    [Fact]
    public void ChooseEarliestOrder_WhenMultipleOptimalSolutions()
    {
        // Arrange
        int[] demand = [1, 1];

        // Act
        // Ordering once or twice has the same cost when holding cost is zero
        var plan = GasStationLotSizing.Solve(demand, 2, 1, 0, 5);

        // Assert
        plan.TotalCost.Should().Be(2 + 2 * 1);
        plan.Orders.Should().ContainSingle();
        plan.Orders[0].Day.Should().Be(1);
        plan.Orders[0].Quantity.Should().Be(2);
    }
}
