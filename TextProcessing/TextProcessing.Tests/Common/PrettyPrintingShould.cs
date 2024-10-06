namespace TextProcessing.Tests.Common;

public class PrettyPrintingShould
{
    [Fact]
    public void PrettyPrint_WhenGivenSimpleTextAndLineLength_ShouldReturnFormattedTextAndCost()
    {
        // Arrange
        string text = "This is a sample text";
        int L = 10;

        // Act
        var (formattedText, cost) = PrettyPrinting.PrettyPrint(text, L);

        // Assert
        formattedText.Should().Be("This is" + Environment.NewLine + "a sample" + Environment.NewLine + "text");
        cost.Should().Be(49);
    }

    [Fact]
    public void PrettyPrint_WhenGivenSimpleTextWithTwoLinesAndLineLength_ShouldReturnFormattedTextAndCost()
    {
        // Arrange
        string text = @"This
                        is a sample text";
        int L = 10;

        // Act
        var (formattedText, cost) = PrettyPrinting.PrettyPrint(text, L);

        // Assert
        formattedText.Should().Be("This is" + Environment.NewLine + "a sample" + Environment.NewLine + "text");
        cost.Should().Be(49);
    }

    [Fact] void PrettyPrint_WhenGivenWordWithMaxLength_ShouldReturnFormattedTextAndCost()
    {
        // Arrange
        string text = "This is a sample text";
        int L = 21;

        // Act
        var (formattedText, cost) = PrettyPrinting.PrettyPrint(text, L);

        // Assert
        formattedText.Should().Be("This is a sample text");
        cost.Should().Be(0);
    }

    [Fact]
    public void PrettyPrint_WhenGivenTextAndLineLength_ShouldReturnFormattedTextAndCost2()
    {
        // Arrange
        string text = @"Call me Ishmael.
                        Some years ago,
                        never mind how long precisely,
                        having little or no money in my purse,
                        and nothing particular to interest me on shore,
                        I thought I would sail about a little and see the watery part of the world.";

        int L = 40;

        // Act
        var (formattedText, cost) = PrettyPrinting.PrettyPrint(text, L);

        // Assert
        var shouldReturn = "Call me Ishmael. Some years ago, never" + Environment.NewLine +
                            "mind how long precisely, having little" + Environment.NewLine +
                            "or no money in my purse, and nothing" + Environment.NewLine +
                            "particular to interest me on shore, I" + Environment.NewLine +
                            "thought I would sail about a little" + Environment.NewLine +
                            "and see the watery part of the world.";

        formattedText.Should().Be(shouldReturn);

        cost.Should().Be(67);
    }
}
