namespace TextProcessing.Common.Models;

public readonly struct Word
{
    public int StartIndex { get; init; }

    public int EndIndex { get; init; }

    public readonly int Length => EndIndex - StartIndex + 1;

    public Word(int startIndex, int endIndex)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
    }

    public readonly ReadOnlySpan<char> GetWordText(ReadOnlySpan<char> text)
    {
        return text.Slice(StartIndex, Length);
    }
}
