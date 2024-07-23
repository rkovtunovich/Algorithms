using System.Numerics;

namespace MathAlgo.Common;

public static class NumberExtensions
{
    public static TSelf Sqrt<TSelf>(this TSelf value) where TSelf : INumber<TSelf>
    {
        // Convert to double, calculate the square root, and then convert back to TSelf
        double doubleValue = Convert.ToDouble(value);
        double sqrtValue = Math.Sqrt(doubleValue);
        
        return TSelf.CreateChecked(sqrtValue);
    }
}
