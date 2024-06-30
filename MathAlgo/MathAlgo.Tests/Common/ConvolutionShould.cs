namespace MathAlgo.Tests.Common;

public class ConvolutionShould
{
    [Fact]
    public void Convolute_SimpleCase()
    {
        // Arrange
        var a = new int[] { 1, 2, 3 };
        var b = new int[] { 4, 5, 6 };
        var expected = new int[] { 4, 13, 28, 27, 18 };

        // Act
        var result = Convolution.Convolute(a, b);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public void Convolute_OneElement()
    {
        // Arrange
        var a = new int[] { 1 };
        var b = new int[] { 2 };
        var expected = new int[] { 2 };

        // Act
        var result = Convolution.Convolute(a, b);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ConvoluteFFT_OneElement()
    {
        // Arrange
        var a = new double[] { 1 };
        var b = new double[] { 2 };
        var expected = new double[] { 2 };

        // Act
        var result = Convolution.ConvoluteFFT(a, b);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ConvoluteFFT_TwoElements()
    {
        // Arrange
        var a = new double[] { 1, 1 };
        var b = new double[] { 2, 2 };
        var expected = new double[] { 2, 4, 2 };

        // Act
        var result = Convolution.ConvoluteFFT(a, b);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ConvoluteFFT_SimpleCase()
    {
        // Arrange
        var a = new double[] { 1, 2, 3 };
        var b = new double[] { 4, 5, 6 };
        var expected = new double[] { 4, 13, 28, 27, 18 };

        // Act
        var result = Convolution.ConvoluteFFT(a, b);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}

