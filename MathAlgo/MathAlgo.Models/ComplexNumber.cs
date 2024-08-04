namespace MathAlgo.Models;

public readonly struct ComplexNumber(double real = 0, double imaginary = 0)
{
    public double Real { get; } = real;

    public double Imaginary { get; } = imaginary;

    public static ComplexNumber operator +(ComplexNumber a, ComplexNumber b)
    {
        return new(a.Real + b.Real, a.Imaginary + b.Imaginary);
    }

    public static ComplexNumber operator -(ComplexNumber a, ComplexNumber b)
    {
        return new(a.Real - b.Real, a.Imaginary - b.Imaginary);
    }

    public static ComplexNumber operator *(ComplexNumber a, ComplexNumber b)
    {
        return new(a.Real * b.Real - a.Imaginary * b.Imaginary, a.Real * b.Imaginary + a.Imaginary * b.Real);
    }

    public static ComplexNumber operator /(ComplexNumber a, ComplexNumber b)
    {
        var denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
        return new((a.Real * b.Real + a.Imaginary * b.Imaginary) / denominator, (a.Imaginary * b.Real - a.Real * b.Imaginary) / denominator);
    }

    public static ComplexNumber operator *(ComplexNumber a, double b)
    {
        return new(a.Real * b, a.Imaginary * b);
    }

    public static ComplexNumber operator /(ComplexNumber a, double b)
    {
        return new(a.Real / b, a.Imaginary / b);
    }

    public static ComplexNumber operator *(double a, ComplexNumber b)
    {
        return new(b.Real * a, b.Imaginary * a);
    }

    public static ComplexNumber operator /(double a, ComplexNumber b)
    {
        var denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
        return new(a * b.Real / denominator, -a * b.Imaginary / denominator);
    }

    public static ComplexNumber operator *(int a, ComplexNumber b)
    {
        return new(b.Real * a, b.Imaginary * a);
    }

    public static ComplexNumber operator /(int a, ComplexNumber b)
    {
        var denominator = b.Real * b.Real + b.Imaginary * b.Imaginary;
        return new(a * b.Real / denominator, -a * b.Imaginary / denominator);
    }

    public static ComplexNumber operator -(ComplexNumber a)
    {
        return new(-a.Real, -a.Imaginary);
    }

    public readonly double Magnitude => Math.Sqrt(Real * Real + Imaginary * Imaginary);

    public readonly ComplexNumber Conjugate => new(Real, -Imaginary);

    public readonly ComplexNumber Inverse => Conjugate / (Real * Real + Imaginary * Imaginary);

    public ComplexNumber ApplyToTolerance(double tolerance = 1e-10)
    {
        double real = Math.Abs(Real - Math.Round(Real)) < tolerance ? Math.Round(Real) : Real;
        double imaginary = Math.Abs(Imaginary - Math.Round(Imaginary)) < tolerance ? Math.Round(Imaginary) : Imaginary;

        return new(real, imaginary);
    }

    public override string ToString()
    {
        if (Imaginary is 0)
            return $"{Real}";

        if (Real is 0)
            return $"{(Imaginary < 0 ? "-" : "")}{GetImaginaryString(Imaginary)}";
        return $"{Real} {GetSign(Imaginary)} {GetImaginaryString(Imaginary)}";
    }

    private static string GetImaginaryString(double imaginary) => imaginary switch
    {
        1 => "i",
        -1 => "i",
        < 0 => $"{-imaginary}i",
        _ => $"{imaginary}i"
    };

    private static string GetSign(double value) => value < 0 ? "-" : "+";
}
