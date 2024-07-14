using Searching.MajorityElementProblem;

namespace Searching.Tests.MajorityElementProblem;

public class BoyerMooreVotingShould
{
    [Theory]
    [InlineData(new int[] { 1, 2, 3, 4, 5 }, false)]
    [InlineData(new int[] { 1, 2, 3, 4, 5, 1, 1, 1, 1, 1 }, true)]
    [InlineData(new int[] { 1, 2, 3, 4, 5, 1, 1, 1, 1, 2 }, false)]
    [InlineData(new int[] { 1, 2, 3, 4, 5, 1, 1, 1, 1, 2, 2, 2, 2, 2, 1 }, false)]
    [InlineData(new int[] { 1, 2, 3, 4, 5, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2 }, true)]
    public void Return_Expected_Result(int[] arr, bool expected)
    {
        var voting = new BoyerMooreVoting<int>();

        bool result = voting.HasMajorityElement(arr);

        result.Should().Be(expected);
    }
}
