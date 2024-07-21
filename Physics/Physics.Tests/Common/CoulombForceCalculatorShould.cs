namespace Physics.Tests.Common;

public class CoulombForceCalculatorShould
{
    [Fact]
    public void ComputeForces_WhenGivenTheSameCharges()
    {
        // Arrange
        double[] charges = { 1.0, 1.0, 1.0, 1.0 };
        double constant = 1.0;
        double[] expectedForces = { -1.36, -0.25, 0.25, 1.36 };
        var calculator = new CoulombForceCalculator(constant, charges);

        // Act
        var forces = calculator.ComputeForces();

        // Assert
        for (int i = 0; i < expectedForces.Length; i++)
        {
            expectedForces[i].Should().BeApproximately(Math.Round(forces[i], 2), 0.01);
        }
    }

    [Fact]
    public void ComputeForces_WhenGivenSimplePositiveCharges()
    {
        // Arrange
        double[] charges = { 3.0, 2.0, 1.0, 2.0, 3.0 };
        double constant = 1.0;
        double[] expectedForces = { -7.98, 2.33, 0.0, -2.33, 7.98 };
        var calculator = new CoulombForceCalculator(constant, charges);

        // Act
        var forces = calculator.ComputeForces();

        // Assert
        for (int i = 0; i < expectedForces.Length; i++)
        {
            expectedForces[i].Should().BeApproximately(Math.Round(forces[i], 2), 0.01);
        }
    }

    [Fact]
    public void ComputeForces_WhenGivenSimpleNegativeCharges()
    {
        // Arrange
        double[] charges = { -3.0, -2.0, -1.0, -2.0, -3.0 };
        double constant = 1.0;
        double[] expectedForces = { -7.98, 2.33, 0.0, -2.33, 7.98 };
        var calculator = new CoulombForceCalculator(constant, charges);

        // Act
        var forces = calculator.ComputeForces();

        // Assert
        for (int i = 0; i < expectedForces.Length; i++)
        {
            expectedForces[i].Should().BeApproximately(Math.Round(forces[i], 2), 0.01);
        }
    }

    [Fact]
    public void ComputeForces_WhenGivenMixedCharges()
    {
        // Arrange
        double[] charges = { -3.0, 2.0, -1.0, 2.0, -3.0 };
        double constant = 1.0;
        double[] expectedForces = { 5.35, -4.33, 0.0, 4.33, -5.35 };
        var calculator = new CoulombForceCalculator(constant, charges);

        // Act
        var forces = calculator.ComputeForces();

        // Assert
        for (int i = 0; i < expectedForces.Length; i++)
        {
            expectedForces[i].Should().BeApproximately(Math.Round(forces[i], 2), 0.01);
        }
    }

    [Fact]
    public void ComputeForces_WhenGivenZeroCharges()
    {
        // Arrange
        double[] charges = { 0.0, 0.0, 0.0, 0.0 };
        double constant = 1.0;
        double[] expectedForces = { 0.0, 0.0, 0.0, 0.0 };
        var calculator = new CoulombForceCalculator(constant, charges);

        // Act
        var forces = calculator.ComputeForces();

        // Assert
        for (int i = 0; i < expectedForces.Length; i++)
        {
            expectedForces[i].Should().BeApproximately(Math.Round(forces[i], 2), 0.01);
        }
    }
}
