namespace Scheduling.Tests.Common;

public class SubsetSumProblemShould
{
    [Fact]
    public void MaximizeProcessingTime_WhenEmptyJobs_ShouldReturnEmptyJobs()
    {
        // Arrange
        var jobs = new List<Job>();
        int timeCapacity = 0;

        // Act
        var result = SubsetSumProblem.MaximizeProcessingTime(jobs, timeCapacity);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void MaximizeProcessingTime_WhenSingleJob_ShouldReturnSingleJob()
    {
        // Arrange
        var jobs = new List<Job>
        {
            new(1, 1)
        };
        int timeCapacity = 1;

        // Act
        var result = SubsetSumProblem.MaximizeProcessingTime(jobs, timeCapacity);

        // Assert
        result.Should().HaveCount(1);
        result.Should().Contain(jobs[0]);
    }

    [Fact]
    public void MaximizeProcessingTime_WhenMultipleJobs_ShouldReturnSelectedJobs()
    {
        // Arrange
        var jobs = new List<Job>
        {
            new(1, 2),
            new(2, 3),
            new(3, 4),
            new(4, 5)
        };
        int timeCapacity = 7;

        // Act
        var result = SubsetSumProblem.MaximizeProcessingTime(jobs, timeCapacity);

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(j => j.Id == 2);
        result.Should().Contain(j => j.Id == 3);
    }
}
