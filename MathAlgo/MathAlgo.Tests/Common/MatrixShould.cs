namespace MathAlgo.Tests.Common;

public class MatrixShould
{
    [Fact]
    public void Mult2x2()
    {
        // arrange
        var m1 = new int[][] { [1, 3], [7, 5] };
        var m2 = new int[][] { [6, 8], [4, 2] };

        // act
        var result = Matrix.Mult(m1, m2);

        // assert
        result.Should().BeEquivalentTo(new int[][] { [18, 14], [62, 66] });
    }

    [Fact]
    public void Mult2x2Rec()
    {
        // arrange
        var m1 = new int[][] { [1, 3], [7, 5] };
        var m2 = new int[][] { [6, 8], [4, 2] };

        // act
        var result = Matrix.MultRec(m1, m2);

        // assert
        result.Should().BeEquivalentTo(new int[][] { [18, 14], [62, 66] });
    }

    [Fact]
    public void Mult2x2Strassen()
    {
        // arrange
        var m1 = new int[][] { [1, 3], [7, 5] };
        var m2 = new int[][] { [6, 8], [4, 2] };

        // act
        var result = Matrix.MultStrassen(m1, m2);

        // assert
        result.Should().BeEquivalentTo(new int[][] { [18, 14], [62, 66] });
    }
}
