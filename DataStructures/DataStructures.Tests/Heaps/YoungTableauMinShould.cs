namespace DataStructures.Tests.Heaps;

public class YoungTableauMinShould
{
    [Fact]
    public void Insert_WhenYoungTableauIsEmpty_ShouldInsertValue()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(3, 3, int.MaxValue);

        // Act
        youngTableau.Insert(1);

        // Assert
        youngTableau.ExtractMin().Should().Be(1);
    }

    [Fact]
    public void Insert_WhenYoungTableauIsFull_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(2, 2, int.MaxValue);

        // Act
        youngTableau.Insert(1);
        youngTableau.Insert(2);
        youngTableau.Insert(3);
        youngTableau.Insert(4);

        // Assert
        Action act = () => youngTableau.Insert(5);
        act.Should().Throw<InvalidOperationException>().WithMessage("Young tableau is full.");
    }

    [Fact]
    public void ExtractMin_WhenYoungTableauIsEmpty_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(2, 2, int.MaxValue);

        // Act
        Action act = () => youngTableau.ExtractMin();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Young tableau is empty.");
    }

    [Fact]
    public void ExtractMin_WhenYoungTableauIsNotEmpty_ShouldExtractMinValue()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(3, 3, int.MaxValue);

        // Act
        youngTableau.Insert(3);
        youngTableau.Insert(2);
        youngTableau.Insert(1);

        // Assert
        youngTableau.ExtractMin().Should().Be(1);
    }

    [Fact]
    public void Search_WhenYoungTableauIsEmpty_ShouldReturnFalse()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(2, 2, int.MaxValue);

        // Act
        var result = youngTableau.Search(1);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Search_WhenYoungTableauIsNotEmptyAndValueExists_ShouldReturnTrue()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(3, 3, int.MaxValue);

        // Act
        youngTableau.Insert(3);
        youngTableau.Insert(2);
        youngTableau.Insert(1);

        // Assert
        youngTableau.Search(1).Should().BeTrue();
    }

    [Fact]
    public void Search_WhenYoungTableauIsNotEmptyAndValueDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(3, 3, int.MaxValue);

        // Act
        youngTableau.Insert(3);
        youngTableau.Insert(2);
        youngTableau.Insert(1);

        // Assert
        youngTableau.Search(4).Should().BeFalse();
    }

    [Fact]
    public void Search_WhenYoungTableauIsNotEmptyAndValueIsSmallerThanAll_ShouldReturnFalse()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(3, 3, int.MaxValue);

        // Act
        youngTableau.Insert(3);
        youngTableau.Insert(2);
        youngTableau.Insert(1);

        // Assert
        youngTableau.Search(0).Should().BeFalse();
    }

    [Fact]
    public void Search_WhenYoungTableauIsNotEmptyAndValueIsLargerThanAll_ShouldReturnFalse()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(3, 3, int.MaxValue);

        // Act
        youngTableau.Insert(3);
        youngTableau.Insert(2);
        youngTableau.Insert(1);

        // Assert
        youngTableau.Search(4).Should().BeFalse();
    }

    [Fact]
    public void Search_WhenYoungTableauIsNotEmptyAndValueIsInTheMiddle_ShouldReturnTrue()
    {
        // Arrange
        var youngTableau = new YoungTableauMin<int>(3, 3, int.MaxValue);

        // Act
        youngTableau.Insert(3);
        youngTableau.Insert(2);
        youngTableau.Insert(1);

        // Assert
        youngTableau.Search(2).Should().BeTrue();
    }
}
