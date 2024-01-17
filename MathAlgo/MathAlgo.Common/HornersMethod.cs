using System.Numerics;

namespace MathAlgo.Common;

public static class HornersMethod<T> where T : INumber<T>
{
    public static T Evaluate(List<T> polynomialCoefficients, T x)
    {
        var result = polynomialCoefficients[0];
        for (var i = 1; i < polynomialCoefficients.Count; i++)
        {
            result = result * x + polynomialCoefficients[i];
        }

        return result;
    }
}
