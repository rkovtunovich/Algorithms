namespace Searching.Tests.Common;

public class MaxSumSubArrayShould
{
    [Fact]
    public void ThrowArgumentExceptionWhenArrayIsNull()
    {
        int[] arr = null;

        Assert.Throws<ArgumentException>(() => MaxSumSubArray.Find(arr));
    }

    [Fact]
    public void ThrowArgumentExceptionWhenArrayIsEmpty()
    {
        int[] arr = [];

        Assert.Throws<ArgumentException>(() => MaxSumSubArray.Find(arr));
    }

    [Theory]
    [InlineData(new int[] { 1 }, 0, 0, 1)]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, 0, 4, 15)]
    [InlineData(new int[] { 1, 2, -3, 4, 5 }, 3, 4, 9)]
    [InlineData(new int[] { -1, -2, -3, -4, -5 }, 0, 0, -1)]
    [InlineData(new int[] { -1, -2, 3, -4, -5 }, 2, 2, 3)]
    [InlineData(new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }, 3, 6, 6)]
    public void ReturnMaxSumSubArray(int[] arr, int start, int end, int sum)
    {
        var result = MaxSumSubArray.Find(arr);

        result.start.Should().Be(start);
        result.end.Should().Be(end);
        result.sum.Should().Be(sum);
    }

    [Theory]
    [InlineData(new int[] { 1 }, 0, 0, 1)]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, 0, 4, 15)]
    [InlineData(new int[] { 1, 2, -3, 4, 5 }, 0, 4, 9)]
    [InlineData(new int[] { -1, -2, -3, -4, -5 }, 0, 0, -1)]
    [InlineData(new int[] { -1, -2, 3, -4, -5 }, 2, 2, 3)]
    [InlineData(new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }, 3, 6, 6)]
    public void ReturnMaxSumSubArrayBruteForce(int[] arr, int start, int end, int sum)
    {
        var result = MaxSumSubArray.FindBruteForce(arr);

        result.start.Should().Be(start);
        result.end.Should().Be(end);
        result.sum.Should().Be(sum);
    }

    [Theory]
    [InlineData(new int[] { 1 }, 0, 0, 1)]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, 0, 4, 15)]
    [InlineData(new int[] { 1, 2, -3, 4, 5 }, 0, 4, 9)]
    [InlineData(new int[] { -1, -2, -3, -4, -5 }, 0, 0, -1)]
    [InlineData(new int[] { -1, -2, 3, -4, -5 }, 2, 2, 3)]
    [InlineData(new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }, 3, 6, 6)]
    public void ReturnMaxSubArrayHybrid(int[] arr, int start, int end, int sum)
    {
        var result = MaxSumSubArray.FindHybrid(arr, 4);

        result.start.Should().Be(start);
        result.end.Should().Be(end);
        result.sum.Should().Be(sum);
    }

    [Theory]
    [InlineData(new int[] { 1 }, 0, 0, 1)]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, 0, 4, 15)]
    [InlineData(new int[] { 1, 2, -3, 4, 5 }, 0, 4, 9)]
    [InlineData(new int[] { -1, -2, -3, -4, -5 }, 0, 0, -1)]
    [InlineData(new int[] { -1, -2, 3, -4, -5 }, 2, 2, 3)]
    [InlineData(new int[] { -2, 1, -3, 4, -1, 2, 1, -5, 4 }, 3, 6, 6)]
    public void ReturnMaxSumSubArrayKadane(int[] arr, int start, int end, int sum)
    {
        var result = MaxSumSubArray.FindKadane(arr);

        result.start.Should().Be(start);
        result.end.Should().Be(end);
        result.sum.Should().Be(sum);
    }
}
