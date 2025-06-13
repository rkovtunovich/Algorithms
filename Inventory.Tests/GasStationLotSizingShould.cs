namespace Inventory.Tests;

public class GasStationLotSizingShould
{
    [Fact]
    public void ReturnEmptyPlan_WhenNoDays()
    {
        var result = GasStationLotSizing.Solve(Array.Empty<int>(), 5, 2, 1, 10);

        result.TotalCost.Should().Be(0);
        result.Orders.Should().BeEmpty();
    }

    [Fact]
    public void HandleSingleDayDemand()
    {
        int[] demand = [7];
        var plan = GasStationLotSizing.Solve(demand, 10, 3, 1, 10);

        plan.TotalCost.Should().Be(10 + 7 * 3);
        plan.Orders.Should().ContainSingle().Which.Day.Should().Be(1);
        plan.Orders.Single().Quantity.Should().Be(7);
    }

    [Fact]
    public void RespectCapacityConstraint()
    {
        int[] demand = [2, 2, 2];
        // capacity only allows ordering one day's demand at a time
        var plan = GasStationLotSizing.Solve(demand, 5, 1, 0.5, 2);

        plan.Orders.Should().HaveCount(3);
        plan.Orders.Should().OnlyContain(o => o.Quantity == 2);
    }

    [Fact]
    public void SkipZeroDemands()
    {
        int[] demand = [0, 0, 3];
        var plan = GasStationLotSizing.Solve(demand, 4, 2, 1, 5);

        plan.Orders.Should().HaveCount(1);
        var order = plan.Orders[0];
        order.Day.Should().Be(3);
        order.Quantity.Should().Be(3);
    }

    [Fact]
    public void HandleDemandsEqualToCapacity()
    {
        int[] demand = [5, 5];
        var plan = GasStationLotSizing.Solve(demand, 1, 1, 1, 5);

        plan.Orders.Should().HaveCount(2);
        plan.Orders[0].Quantity.Should().Be(5);
        plan.Orders[1].Quantity.Should().Be(5);
    }

    [Fact]
    public void ChooseEarliestOrder_WhenMultipleOptimalSolutions()
    {
        int[] demand = [1, 1];
        // Ordering once or twice has the same cost when holding cost is zero
        var plan = GasStationLotSizing.Solve(demand, 2, 1, 0, 5);

        plan.TotalCost.Should().Be(2 + 2 * 1);
        plan.Orders.Should().ContainSingle();
        plan.Orders[0].Day.Should().Be(1);
        plan.Orders[0].Quantity.Should().Be(2);
    }
}
