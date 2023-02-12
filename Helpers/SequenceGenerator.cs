namespace Helpers;

public static class SequenceGenerator<T>
{
    public static List<T> Generate(List<T> alphabet, int length)
    {
        var random = new Random();
        var sequence = new List<T>();

        for (int i = 0; i < length; i++)
        {
            sequence.Add(alphabet[random.Next(0, alphabet.Count)]);   
        }

        return sequence;
    }
}
