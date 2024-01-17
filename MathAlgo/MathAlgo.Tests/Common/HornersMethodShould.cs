using MathAlgo.Common;

namespace MathAlgo.Tests.Common;

public class HornersMethodShould
{
    [Fact]
    public void EvaluatePolynomial()
    {
        var polynomial = new List<int>
        {
            2,
            5,
            -1,
            3,
            2
        };

        var result = HornersMethod<int>.Evaluate(polynomial, 10);

        result.Should().Be(24932);
    }
}
