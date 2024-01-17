using Sorting.Common;

namespace Sorting.Tests.Common;

public class ArrayInversionsShould
{
    [Fact]
    public void CountInversions()
    {
        var array = new int[] { 2, 3, 8, 6, 1 };

        var result = ArrayInversions.Count(ref array);

        result.Should().Be(5);
    }
}
