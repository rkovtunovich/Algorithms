using Scheduling.Common.Allocation;

namespace Scheduling.Tests.Common.Allocation;

public class ResourcePlacementShould
{
    [Fact]
    public void Optimize_WhenOneNode_ShouldReturnSinglePlacementNode()
    {
        // Arrange
        int numNodes = 1;

        // Define placement cost function
        static double placementCost(int i) => 1;

        // Define access cost function (distance-based)
        static double accessCost(int from, int to) => Math.Abs(to - from);

        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize2(numNodes, placementCost, accessCost);

        // Assert
        minCost.Should().Be(1);
        placementNodes.Should().BeEquivalentTo([1]);
    }

    [Fact]
    public void Optimize_WhenTwoNodes_ShouldReturnOptimalPlacement()
    {
        // Arrange
        int numNodes = 2;
        static double placementCost(int i) => i;
        static double accessCost(int from, int to) => Math.Abs(to - from);
        
        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize2(numNodes, placementCost, accessCost, true);
        
        // Assert
        minCost.Should().Be(2);
        placementNodes.Should().BeEquivalentTo([1, 2]);
    }

    [Fact]
    public void Optimize_WhenGivenNodes_ShouldReturnOptimalPlacement()
    {
        // Arrange
        int numNodes = 5;

        // Define placement cost function
        static double placementCost(int i) => i switch
        {
            1 => 3,
            2 => 5,
            3 => 2,
            4 => 4,
            _ => 1 
        };

        // Define access cost function (distance-based)
        static double accessCost(int from, int to) => Math.Abs(to - from);

        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize2(numNodes, placementCost, accessCost);

        // Assert
        minCost.Should().Be(8);
        placementNodes.Should().BeEquivalentTo([1, 3, 5]);
    }

    [Fact]
    public void Optimize_WhenPlacementCostsAreZero_ShouldPlaceAtAllNodes()
    {
        // Arrange
        int numNodes = 5;
        static double placementCost(int i) => 0;
        static double accessCost(int from, int to) => 1; // Constant access cost

        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize2(numNodes, placementCost, accessCost);

        // Assert
        minCost.Should().Be(0);
        placementNodes.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Optimize_WhenAccessCostsAreHigh_ShouldPlaceResourcesFrequently()
    {
        // Arrange
        int numNodes = 4;
        static double placementCost(int i) => 2; // Constant placement cost
        static double accessCost(int from, int to) => 10 * (to - from); // High access cost

        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize(numNodes, placementCost, accessCost);

        // Assert
        minCost.Should().Be(8); // Place at every node
        placementNodes.Should().BeEquivalentTo([1, 2, 3, 4]);
    }

    [Fact]
    public void Optimize_WhenPlacementCostsIncrease_ShouldBalancePlacementAndAccessCosts()
    {
        // Arrange
        int numNodes = 6;
        static double placementCost(int i) => i * 2; // Increasing placement cost
        static double accessCost(int from, int to) => Math.Pow(to - from, 2); // Quadratic access cost

        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize2(numNodes, placementCost, accessCost);

        // Assert
        minCost.Should().Be(20); // Expected minimum cost
        placementNodes.Should().BeEquivalentTo([1, 4]);
    }

    [Fact]
    public void Optimize_WhenAccessCostsAreNonlinear_ShouldOptimizePlacements()
    {
        // Arrange
        int numNodes = 5;
        static double placementCost(int i) => 1; // Low placement cost
        static double accessCost(int from, int to) => Math.Exp(to - from) - 1; // Exponential access cost

        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize2(numNodes, placementCost, accessCost);

        // Assert
        minCost.Should().BeApproximately(5, 0.001); // Place at every node due to high access cost
        placementNodes.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void Optimize_WhenSingleNodeWithHighPlacementCost_ShouldReturnCorrectCost()
    {
        // Arrange
        int numNodes = 1;
        static double placementCost(int i) => 100; // High placement cost
        static double accessCost(int from, int to) => 0; // No access cost

        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize(numNodes, placementCost, accessCost);

        // Assert
        minCost.Should().Be(100);
        placementNodes.Should().BeEquivalentTo([1]);
    }

    [Fact]
    public void Optimize_WithVaryingPlacementAndAccessCosts_ShouldFindOptimalPlacements()
    {
        // Arrange
        int numNodes = 7;
        static double placementCost(int i) => i % 2 is 0 ? 3 : 1; // Lower cost at odd nodes
        static double accessCost(int from, int to) => (to - from) * (from % 2 is 0 ? 2 : 1); // Variable access cost

        // Act
        var (minCost, placementNodes) = ResourcePlacement.Optimize2(numNodes, placementCost, accessCost);

        // Assert
        minCost.Should().Be(7);
        placementNodes.Should().BeEquivalentTo([1, 3, 5, 7]);
    }
}
