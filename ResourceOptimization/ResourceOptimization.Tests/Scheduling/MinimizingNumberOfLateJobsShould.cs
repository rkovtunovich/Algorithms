using ResourceOptimization.Scheduling;

namespace ResourceOptimization.Tests.Scheduling;

public class MinimizingNumberOfLateJobsShould
{
    [Fact]
    public void Schedule_WhenNoJobs_ReturnsEmptyLists()
    {
        // Arrange
        var jobs = new List<Job>();

        // Act
        var (onTime, late) = MinimizingNumberOfLateJobs.Schedule(jobs);

        // Assert
        onTime.Should().BeEmpty();
        late.Should().BeEmpty();
    }

    [Fact]
    public void Schedule_WhenAllJobsFit_ReturnsAllOnTime()
    {
        // Arrange
        var jobs = new List<Job>
        {
            new(1, 1, 2),
            new(2, 1, 3),
            new(3, 1, 4)
        };

        // Act
        var (onTime, late) = MinimizingNumberOfLateJobs.Schedule(jobs);

        // Assert
        onTime.Should().BeEquivalentTo(jobs.OrderBy(j => j.Deadline));
        late.Should().BeEmpty();
    }

    [Fact]
    public void Schedule_WhenSomeJobsLate_SplitsCorrectly()
    {
        // Arrange
        var jobs = new List<Job>
        {
            new(1, 3, 4),
            new(2, 5, 5),
            new(3, 2, 6)
        };

        // Act
        var (onTime, late) = MinimizingNumberOfLateJobs.Schedule(jobs);

        // Assert
        onTime.Should().BeEquivalentTo(new List<Job>
        {
            new(1, 3, 4),
            new(3, 2, 6)
        });
        late.Should().BeEquivalentTo(new List<Job>
        {
            new(2, 5, 5)
        });
    }
}

