namespace MathAlgo.Common;

public static class PrimeFactors
{
    public static IEnumerable<int> GetPrimeFactors(int number)
    {
        if (number < 2)
            throw new ArgumentException("Number must be greater than 1");

        var factors = new List<int>();
        var divisor = 2;

        while (number > 1)
        {
            if (number % divisor == 0)
            {
                factors.Add(divisor);
                number /= divisor;
            }
            else
            {
                divisor++;
            }
        }

        return factors;
    }
}
