namespace Searching.Tests.Common;

public class ArrayInversionsShould
{
    [Fact]
    public void CountInversions()
    {
        var array = new int[] { 2, 3, 8, 6, 1 };

        var result = ArrayInversions.Count(ref array);

        result.Should().Be(5);
    }

    [Fact]
    public void CountInversionsEmptyArray()
    {
        var array = Array.Empty<int>();

        var result = ArrayInversions.Count(ref array);

        result.Should().Be(0);
    }

    [Fact]
    public void CountInversionsSingleElementArray()
    {
        var array = new int[] { 1 };

        var result = ArrayInversions.Count(ref array);

        result.Should().Be(0);
    }

    [Fact]
    public void CountInversionsSortedArray()
    {
        var array = new int[] { 1, 2, 3, 4, 5 };

        var result = ArrayInversions.Count(ref array);

        result.Should().Be(0);
    }

    [Fact]
    public void CountInversionsReverseSortedArray()
    {
        var array = new int[] { 5, 4, 3, 2, 1 };

        var result = ArrayInversions.Count(ref array);

        result.Should().Be(10);
    }

    [Fact]
    public void CountInversionsArrayWithDuplicates()
    {
        var array = new int[] { 1, 2, 1, 2, 1 };

        var result = ArrayInversions.Count(ref array);

        result.Should().Be(3);
    }
}
