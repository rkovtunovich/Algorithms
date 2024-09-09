namespace Scheduling.Tests.Common;

public class HighLowStressJobsShould
{
    [Fact]
    public void FinsMaxValue_WhenGivenLowStressAndHighStress_ShouldReturnMaxValue()
    {
        // Arrange
        int[] lowStress = { 10, 1, 10, 10 };
        int[] highStress = { 5, 50, 45, 1 };

        // Act
        int result = HighLowStressJobs.FindMaxValue(lowStress, highStress);

        // Assert
        result.Should().Be(70);
    }
}
