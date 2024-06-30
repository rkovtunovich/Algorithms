using Models.MathModels;

namespace MathAlgo.Common;

public static class Convolution
{
    public static int[] Convolute(int[] a, int[] b)
    {
        var n = a.Length;
        var m = b.Length;
        var result = new int[n + m - 1];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < m; j++)
            {
                result[i + j] += a[i] * b[j];
            }
        }
        return result;
    }

    public static double[] ConvoluteFFT(double[] a, double[] b)
    {
        // 1. coefficient representation to point-value representation (evaluating polynomials at roots of unity)
        var n = a.Length;
        var m = b.Length;

        // 1.1 pad the size to power of 2, using bit manipulation
        var resultSize = n + m - 1;
        var paddedSize = (int)Math.Pow(2, Math.Ceiling(Math.Log2(resultSize)));
        var paddedCoefficientsA = GetPaddedCoefficients(a, paddedSize);
        var paddedCoefficientsB = GetPaddedCoefficients(b, paddedSize);

        // 1.2 Discrete Fourier Transform
        FastFourierTransformation.DFTIterative(paddedCoefficientsA, false);
        FastFourierTransformation.DFTIterative(paddedCoefficientsB, false);

        // 2. multiply point-value representations
        MultiplyPointValueRepresentations(paddedCoefficientsA, paddedCoefficientsB);

        // 3. point-value representation to coefficient representation (interpolating polynomials from values at roots of unity)
        FastFourierTransformation.DFTIterative(paddedCoefficientsA, true);

        // 4. extract real parts
        var result = ExtractRealParts(paddedCoefficientsA, resultSize);
       
        return result;
    }

    private static ComplexNumber[] GetPaddedCoefficients(double[] coefficients, int length)
    {
        // 1. expend the length to power of 2
        length = (int)Math.Pow(2, Math.Ceiling(Math.Log2(length)));

        // 2. pad the coefficients with zeros
        var paddedCoefficients = new ComplexNumber[length];
        for (var i = 0; i < coefficients.Length; i++)
        {
            paddedCoefficients[i] = new ComplexNumber(coefficients[i], 0);
        }

        return paddedCoefficients;
    }

    private static void MultiplyPointValueRepresentations(ComplexNumber[] a, ComplexNumber[] b)
    {
        var n = a.Length;
        for (var i = 0; i < n; i++)
        {
            a[i] *= b[i];
        }
    }

    private static double[] ExtractRealParts(ComplexNumber[] coefficients, int resultSize)
    {
        var result = new double[resultSize];
        for (var i = 0; i < resultSize; i++)
        {
            result[i] = coefficients[i].Real;
        }

        return result;
    }
}
