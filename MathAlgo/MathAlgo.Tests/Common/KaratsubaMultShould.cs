namespace MathAlgo.Tests.Common;

public class KaratsubaMultShould
{
    [Theory]
    [InlineData(0, 1, 0)]
    [InlineData(1, 1, 1)]
    [InlineData(10, 0, 0)]
    [InlineData(10, 1, 10)]
    [InlineData(10, 10, 100)]
    [InlineData(10, 11, 110)]
    [InlineData(11, 11, 121)]
    [InlineData(22, 22, 484)]
    [InlineData(111, 111, 12321)]
    [InlineData(222, 222, 49284)]
    [InlineData(1234, 5678, 7006652)]
    [InlineData(12345678, 87654321, 1082152022374638)]
    public void Mult(int x, int y, long expected)
    {
        // Arrange & Act
        long result = KaratsubaMult.Mult(x, y);

        // Assert
        result.Should().Be(expected);
    }
}
