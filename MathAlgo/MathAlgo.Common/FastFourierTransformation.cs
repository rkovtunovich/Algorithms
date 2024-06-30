namespace MathAlgo.Common;

// Iterative FFT
// Advantages:
// In-Place Computation: The iterative version performs the computation in-place, which can save memory since it doesn't require additional arrays for intermediate results.
// Efficiency: Generally, iterative algorithms have lower overhead than recursive ones because they avoid the overhead of multiple function calls.
// Easier to Parallelize: The iterative nature makes it easier to parallelize on modern multi-core processors.
// Disadvantages:
// Complexity: The iterative version can be more complex to understand and implement correctly, especially the bit-reversal permutation step.
// Memory Access Patterns: The bit-reversal step can lead to non-contiguous memory access patterns, which might be less cache-friendly.
// Recursive FFT
// Advantages:
// Simplicity: The recursive version is often easier to understand and implement. It directly follows the divide-and-conquer approach of the FFT algorithm.
// Natural Divide-and-Conquer: The recursion naturally expresses the divide-and-conquer strategy, which is the core idea behind the FFT.
// Clean Memory Access Patterns: The recursive approach can have more cache-friendly memory access patterns due to the natural subdivision of the problem.
// Disadvantages:
// Stack Overhead: Recursive algorithms can suffer from stack overflow for large input sizes due to the large number of recursive calls.
// Extra Memory Usage: The recursive version typically requires additional memory for the intermediate arrays (even and odd), which can be significant for large inputs.
// Performance Overhead: The overhead of recursive function calls can be non-trivial, especially for deep recursion.
// Detailed Comparison
// 1. Algorithm Complexity and Readability
// Iterative Version: More complex due to explicit bit-reversal and in-place computations. Harder to understand at first glance.
// Recursive Version: Simpler and more readable, follows the natural divide-and-conquer strategy of the FFT.
// 2. Memory Usage
// Iterative Version: In-place, so no additional memory is required beyond the input array.
// Recursive Version: Requires additional memory for the intermediate even and odd arrays at each level of recursion.
// 3. Performance and Overhead
// Iterative Version: Generally faster due to lower overhead from function calls. Bit-reversal can impact cache performance.
// Recursive Version: Slower due to the overhead of recursive calls and extra memory allocation. Better cache performance due to contiguous memory access in the divide-and-conquer steps.
// 4. Scalability and Parallelization
// Iterative Version: Easier to parallelize since the iterative structure maps well to parallel computation paradigms.
// Recursive Version: Can also be parallelized, but requires more sophisticated techniques to manage recursive task splitting.
public static class FastFourierTransformation
{
    /// <summary>
    /// Computes the Discrete Fourier Transform (DFT) of the given array using an iterative Fast Fourier Transform (FFT) algorithm.
    /// </summary>
    /// <param name="coefficients">Array of complex numbers representing the input signal.</param>
    /// <param name="invert">If true, computes the inverse FFT.</param>
    public static void DFTIterative(ComplexNumber[] coefficients, bool invert)
    {
        // Cooley-Tukey FFT algorithm
        int n = coefficients.Length;
        if (n <= 1)
            return;

        // Compute the bit length required to represent the length of the array
        int logN = (int)Math.Log2(n);

        BitReversalPermutation(coefficients, n, logN);

        // Main loop of the Cooley-Tukey algorithm
        // The outer loop iterates over the length of the sub-sequences
        for (int len = 2; len <= n; len <<= 1)
        {
            // Compute the angle for the twiddle factor
            double angle = 2 * Math.PI / len * (invert ? -1 : 1);

            // Create a complex number representing the twiddle factor
            var omega = new ComplexNumber(Math.Cos(angle), Math.Sin(angle)).ApplyToTolerance();

            // Iterate over the sub-sequences
            // The sub-sequences are of length len
            for (int i = 0; i < n; i += len)
            {
                // Initialize the twiddle factor 
                // The first element of the sub-sequence has a twiddle factor of 1
                // Because e^(i*0) = 1
                var w = new ComplexNumber(1, 0);

                // Iterate over the first half of the sub-sequence
                for (int j = 0; j < len / 2; ++j)
                {
                    // Butterfly operation
                    var u = coefficients[i + j];
                    var v = coefficients[i + j + len / 2] * w;
                    coefficients[i + j] = (u + v).ApplyToTolerance();
                    coefficients[i + j + len / 2] = (u - v).ApplyToTolerance();

                    // Update the twiddle factor
                    w *= omega;
                }
            }
        }

        if (!invert)
            return;

        // Normalize the coefficients in case of inverse FFT
        for (int i = 0; i < n; ++i)
            coefficients[i] /= n;

    }

    public static void BitReversalPermutation(ComplexNumber[] coefficients, int n, int logN)
    {
        // Bit-reversal permutation
        // Reorder the input array in bit-reversed order
        // This is done to ensure that the input array is in the correct order for the FFT algorithm
        // For example, for n = 8, initially the array is [0, 1, 2, 3, 4, 5, 6, 7]
        // After bit-reversal permutation, the array becomes [0, 4, 2, 6, 1, 5, 3, 7]
        for (int i = 0; i < n; ++i)
        {
            int j = ReverseBits(i, logN);

            if (i < j)
                (coefficients[j], coefficients[i]) = (coefficients[i], coefficients[j]);
        }
    }

    /// <summary>
    /// Reverses the bits of the given number based on the specified bit size.
    /// </summary>
    /// <param name="num">The number to reverse bits for.</param>
    /// <param name="bitSize">The number of bits to consider for reversal.</param>
    /// <returns>The number with its bits reversed.</returns>
    private static int ReverseBits(int num, int bitSize)
    {
        // Reverses the bits of a number
        int reversed = 0;

        // Iterate over the number of bits in the number
        for (int i = 0; i < bitSize; ++i)
        {
            // Shift the reversed number to the left
            if ((num & (1 << i)) is not 0)            
                reversed |= 1 << (bitSize - 1 - i);            
        }

        return reversed;
    }

    /// <summary>
    /// Computes the Discrete Fourier Transform (DFT) of the given array using a recursive Fast Fourier Transform (FFT) algorithm.
    /// </summary>
    /// <param name="coefficients">Array of complex numbers representing the input signal.</param>
    /// <param name="invert">If true, computes the inverse FFT.</param>
    public static void DFTRecursive(ComplexNumber[] coefficients, bool invert)
    {
        int n = coefficients.Length;

        // Base case: if the length of the array is 1, return
        if (n is 1)
            return;

        // Split the array into even and odd parts
        var even = new ComplexNumber[n / 2];
        var odd = new ComplexNumber[n / 2];

        // Populate the even and odd arrays
        for (int i = 0; i < n / 2; i++)
        {
            even[i] = coefficients[2 * i];
            odd[i] = coefficients[2 * i + 1];
        }

        // Recursively compute the DFT for the even and odd parts
        DFTRecursive(even, invert);
        DFTRecursive(odd, invert);

        // Calculate the twiddle factor
        double angle = 2 * Math.PI / n * (invert ? -1 : 1);

        // Create a complex number representing the twiddle factor
        var w = new ComplexNumber(1, 0);
        var wn = new ComplexNumber(Math.Cos(angle), Math.Sin(angle)).ApplyToTolerance();

        // Combine the results of the even and odd parts
        for (int i = 0; i < n / 2; i++)
        {
            // Butterfly operation
            var u = even[i];
            var v = odd[i] * w;
            coefficients[i] = (u + v).ApplyToTolerance();
            coefficients[i + n / 2] = (u - v).ApplyToTolerance();

            // Update the twiddle factor
            w *= wn;
        }

        if (!invert)        
            return;
        
        for (int i = 0; i < n; i++)      
            coefficients[i] /= n;     
    }
}