namespace Graphs.Core;

public static class GraphFileManager
{
    public static void SaveToFile(string fileName, string dotString)
    {
        using var streamWriter = new StreamWriter(fileName);
        streamWriter.Write(dotString);
        streamWriter.Close();
    }

    public static string ReadFromFile(string fileName)
    {
        using var streamReader = new StreamReader(fileName);
        var dotString = streamReader.ReadToEnd();
        streamReader.Close();

        return dotString;
    }
}
