using Searching.PatternMatching;
using View;

namespace ExamplesRunning;

public static class PatternMatchingExample
{
    public static void RunKnuthMorrisPratt()
    {
        var srt = "babcbabcabcaabcabcabcacabc";
        var pattern = "abcabcacab";

        var result = KnuthMorrisPratt.Match(srt, pattern);

        Viewer.ShowArray(result.ToArray());
    }
}
