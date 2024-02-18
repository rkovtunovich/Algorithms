using ScheduleOptimization.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScheduleOptimization.Tests;

public class IntervalSchedulingMaximizationProblemShould 
{
    // Expected Output: All intervals can be selected since none overlap.
    [Fact]
    public void GetOptimalSetWithDailyPeriods_NonOverlappingIntervals()
    {
        // Arrange
        var intervals = new List<DailyInterval>
        {
            new(10, 14),
            new(15, 19),
            new(20, 23),
        };

        // Act
        var result = IntervalSchedulingMaximizationProblem.GetOptimalSetForCycledTimeline(intervals);
        
        // Assert
        result.Should().HaveCount(3);
    }

    // Expected Output: optimal set is [(9, 12), (14, 18)].
    [Fact]
    public void GetOptimalSetWithDailyPeriods_OverlappingIntervalsNotSpanningMidnight()
    {
        // Arrange
        var intervals = new List<DailyInterval>
        {
            new(9, 12),
            new(11, 15),
            new(14, 18),
        };

        // Act
        var result = IntervalSchedulingMaximizationProblem.GetOptimalSetForCycledTimeline(intervals);
        
        // Assert
        result.Should().HaveCount(2);     
    }

    // Expected Output: [(22, 2), (5, 8)].
    [Fact]
    public void GetOptimalSetWithDailyPeriods_IntervalsSpanningMidnight()
    {
        // Arrange
        var intervals = new List<DailyInterval>
        {
            new(22, 2),
            new(1, 3),
            new(5, 8),
        };

        // Act
        var result = IntervalSchedulingMaximizationProblem.GetOptimalSetForCycledTimeline(intervals);
        
        // Assert
        result.Should().HaveCount(2);
    }

    // Expected Output: [(21, 4), (13, 19)] as these do not overlap and cover the span efficiently.
    [Fact]
    public void GetOptimalSetWithDailyPeriods_MixedIntervalsWithSpanningAndNonSpanningMidnight()
    {
        // Arrange
        var intervals = new List<DailyInterval>
        {
            new(18, 6),
            new(21, 4),
            new(3, 14),
            new(13, 19),
        };

        // Act
        var result = IntervalSchedulingMaximizationProblem.GetOptimalSetForCycledTimeline(intervals);
        
        // Assert
        result.Should().HaveCount(2);
    }

    //  All Intervals Overlap
    // Input: [(1, 23), (2, 22), (3, 21)]
    // Expected Output: Only one interval can be selected due to the large overlap, any could be valid.
    [Fact]
    public void GetOptimalSetWithDailyPeriods_AllIntervalsOverlap()
    {
        // Arrange
        var intervals = new List<DailyInterval>
        {
            new(1, 23),
            new(2, 22),
            new(3, 21),
        };

        // Act
        var result = IntervalSchedulingMaximizationProblem.GetOptimalSetForCycledTimeline(intervals);
        
        // Assert
        result.Should().HaveCount(1);
    }

    // Complex Case with Multiple Valid Solutions
    // Input: [(18, 6), (0, 12), (13, 20), (21, 3)]
    // Expected Output: One possible optimal set is [(0, 12), (13, 20)] or [(18, 6), (13, 20)] or [(13, 20), (21, 3)].
    [Fact]
    public void GetOptimalSetWithDailyPeriods_ComplexCaseWithMultipleValidSolutions()
    {
        // Arrange
        var intervals = new List<DailyInterval>
        {
            new(18, 6),
            new(0, 12),
            new(13, 20),
            new(21, 3),
        };

        // Act
        var result = IntervalSchedulingMaximizationProblem.GetOptimalSetForCycledTimeline(intervals);
        
        // Assert
        result.Should().HaveCount(2);
    }

    // Edge Cases with Start and End at the Same Time
    // Input: [(0, 24), (12, 12), (6, 18)]
    // Expected Output: [(12, 12) or (6, 18)] or just [(0, 24)] if interpreted as a full-day job.
    [Fact]
    public void GetOptimalSetWithDailyPeriods_EdgeCasesWithStartAndEndAtTheSameTime()
    {
        // Arrange
        var intervals = new List<DailyInterval>
        {
            new(0, 24),
            new(12, 12),
            new(6, 18),
        };

        // Act
        var result = IntervalSchedulingMaximizationProblem.GetOptimalSetForCycledTimeline(intervals);
        
        // Assert
        result.Should().HaveCount(1);
    }
}