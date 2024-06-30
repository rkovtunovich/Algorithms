namespace MathAlgo.Tests.Common;

public class FastFourierTransformationShould
{
    [Fact]
    public void DFTIterative_OneElement()
    {
        // Arrange
        var coefficients = new ComplexNumber[]
        {
            new(1, 0),
        };

        // Act
        FastFourierTransformation.DFTIterative(coefficients, false);

        // Assert
        coefficients[0].Real.Should().BeApproximately(1, 1e-6);
    }

    [Fact]
    public void DFTIterative_TwoElements()
    {
        // Arrange
        var coefficients = new ComplexNumber[]
        {
            new(1, 0),
            new(1, 0),
        };

        // Act
        FastFourierTransformation.DFTIterative(coefficients, false);

        // Assert
        coefficients[0].Real.Should().BeApproximately(2, 1e-6);
    }

    [Fact]
    public void DFTIterative_FourElements()
    {
        // Arrange
        var coefficients = new ComplexNumber[]
        {
            new(1, 0),
            new(2, 0),
            new(3, 0),
            new(4, 0),
        };
        var expected = new ComplexNumber[]
        {
            new(10, 0),
            new(-2, 2),
            new(-2, 0),
            new(-2, -2),
        };

        // Act
        FastFourierTransformation.DFTIterative(coefficients, false);

        // Assert
        coefficients.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void BitReversalPermutation_OneElement()
    {
        // Arrange
        var coefficients = new ComplexNumber[]
        {
            new(1, 0),
            new(2, 0),
            new(3, 0),
            new(4, 0),
            new(5, 0),
            new(6, 0),
            new(7, 0),
            new(8, 0)
        };

        var expected = new ComplexNumber[]
        {
            new(1, 0),
            new(5, 0),
            new(3, 0),
            new(7, 0),
            new(2, 0),
            new(6, 0),
            new(4, 0),
            new(8, 0)
        };

        // Act
        FastFourierTransformation.BitReversalPermutation(coefficients, 1, 0);

        // Assert
        coefficients.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void DFTRecursive_OneElement()
    {
        // Arrange
        var coefficients = new ComplexNumber[]
        {
            new(1, 0),
        };

        // Act
        FastFourierTransformation.DFTRecursive(coefficients, false);

        // Assert
        coefficients[0].Real.Should().BeApproximately(1, 1e-6);
    }

    [Fact]
    public void DFTRecursive_TwoElements()
    {
        // Arrange
        var coefficients = new ComplexNumber[]
        {
            new(1, 0),
            new(1, 0),
        };

        var expected = new ComplexNumber[]
        {
            new(2, 0),
            new(0, 0),
        };

        // Act
        FastFourierTransformation.DFTRecursive(coefficients, false);

        // Assert
        coefficients.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void DFTRecursive_FourElements()
    {
        // Arrange
        var coefficients = new ComplexNumber[]
        {
            new(1, 0),
            new(2, 0),
            new(3, 0),
            new(4, 0),
        };
        var expected = new ComplexNumber[]
        {
            new(10, 0),
            new(-2, 2),
            new(-2, 0),
            new(-2, -2),
        };

        // Act
        FastFourierTransformation.DFTRecursive(coefficients, false);

        // Assert
        coefficients.Should().BeEquivalentTo(expected);
    }
}
