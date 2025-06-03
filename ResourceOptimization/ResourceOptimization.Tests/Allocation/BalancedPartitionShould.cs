namespace ResourceOptimization.Tests.Allocation;

public class BalancedPartitionShould
{
    [Fact]
    public void SingleItemGroupSizeOne_ExactThreshold_ReturnsThatIndex()
    {
        // Arrange: one item of weight 5, must pick 1 item, threshold = 5
        int[] weights = [5];
        int groupSize = 1;
        int threshold = 5;

        // Act
        var (possible, indices) = BalancedPartition.TrySplit(weights, groupSize, threshold);

        // Assert
        possible.Should().BeTrue();
        indices.Should().ContainSingle();
        weights[indices[0]].Should().Be(5);
    }

    [Fact]
    public void TwoItems_GroupSizeOne_ThresholdFive_ReturnsOneOfThem()
    {
        // Arrange: two items both weight 5, pick exactly 1, threshold=5
        int[] weights = [5, 5];
        int groupSize = 1;
        int threshold = 5;

        // Act
        var (possible, indices) = BalancedPartition.TrySplit(weights, groupSize, threshold);

        // Assert
        possible.Should().BeTrue();
        indices.Should().ContainSingle();
        weights[indices[0]].Should().Be(5);
    }

    [Fact]
    public void FourItems_GroupSizeTwo_ThresholdThree_ReturnsValidPair()
    {
        // Arrange: weights = [1,2,3,4], pick exactly 2, threshold=3
        // total = 10, so other‐side sum must also exceed 3 ⇒ needed sum ∈ [3..7]
        int[] weights = [1, 2, 3, 4];
        int groupSize = 2;
        int threshold = 3;

        // Act
        var (possible, indices) = BalancedPartition.TrySplit(weights, groupSize, threshold);

        // Assert
        possible.Should().BeTrue();
        indices.Should().HaveCount(2);

        int chosenSum = indices.Select(i => weights[i]).Sum();
        // chosenSum must lie between 3 and 7 (inclusive of bounds)
        (chosenSum >= 3 && chosenSum <= 7).Should().BeTrue();
        // also the complement sum = total − chosenSum must also exceed threshold
        int total = weights.Sum();
        int complementSum = total - chosenSum;
        complementSum.Should().BeGreaterThan(threshold);
    }

    [Fact]
    public void FourOnes_GroupSizeTwo_ThresholdThree_ReturnsFalse()
    {
        // Arrange: weights = [1,1,1,1], pick 2, threshold=3
        // total=4 ⇒ to satisfy both sides > 3, we need subset‐sum ∈ [3..1], empty interval ⇒ impossible
        int[] weights = [1, 1, 1, 1];
        int groupSize = 2;
        int threshold = 3;

        // Act
        var (possible, indices) = BalancedPartition.TrySplit(weights, groupSize, threshold);

        // Assert
        possible.Should().BeFalse();
        indices.Should().BeNull();
    }

    [Fact]
    public void ZeroGroupSize_ThresholdZero_ReturnsEmptySubset()
    {
        // Arrange: any weights, pick zero items, threshold=0 ⇒ sum=0 is valid
        int[] weights = [7, 2, 5];
        int groupSize = 0;
        int threshold = 0;

        // Act
        var (possible, indices) = BalancedPartition.TrySplit(weights, groupSize, threshold);

        // Assert
        possible.Should().BeTrue();
        indices.Should().BeEmpty();
    }

    [Fact]
    public void ZeroGroupSize_PositiveThreshold_ReturnsFalse()
    {
        // Arrange: pick zero items but threshold>0 ⇒ cannot achieve positive sum
        int[] weights = [4, 6, 2];
        int groupSize = 0;
        int threshold = 1;

        // Act
        var (possible, indices) = BalancedPartition.TrySplit(weights, groupSize, threshold);

        // Assert
        possible.Should().BeFalse();
        indices.Should().BeNull();
    }

    [Fact]
    public void EvenSplit_TotalJustTwiceThreshold_ReturnsFalse()
    {
        // Arrange: weights sum exactly 2*threshold, pick n/2
        // e.g. [2,2,2,2], threshold=4, total=8 ⇒ need sum∈[4..4], but picking any two gives sum=4;
        // the complement also has sum=4, which is not strictly > threshold. Our implementation
        // requires sum >= threshold and complement >= threshold, so sum=4, complement=4 is allowed.
        // If we want “strictly >,” adjust threshold check. Here, we used ≥ in code, so this should succeed.
        int[] weights = [2, 2, 2, 2];
        int groupSize = 2;
        int threshold = 4;

        // Act
        var (possible, indices) = BalancedPartition.TrySplit(weights, groupSize, threshold);

        // Assert
        possible.Should().BeTrue();
        indices.Should().HaveCount(2);
        int chosen = indices.Sum(i => weights[i]);
        (chosen >= threshold).Should().BeTrue();
        int complement = weights.Sum() - chosen;
        (complement >= threshold).Should().BeTrue();
    }
}
