namespace TextProcessing.Sorting.External;

public static class FileSizeHelper
{
    public static string Format(long sizeInBytes) => sizeInBytes switch
    {
        >= 1024L * 1024 * 1024 => $"{sizeInBytes / (1024L * 1024 * 1024)} GB",
        >= 1024L * 1024 => $"{sizeInBytes / (1024L * 1024)} MB",
        >= 1024L => $"{sizeInBytes / 1024L} KB",
        _ => $"{sizeInBytes} bytes"
    };

    public static bool ParseSize(string sizeStr, out long sizeInBytes)
    {
        sizeInBytes = 0;
        sizeStr = sizeStr.Trim().ToUpper();
        long multiplier = 1;

        if (sizeStr.EndsWith("GB"))
        {
            multiplier = 1024L * 1024 * 1024;
            sizeStr = sizeStr[..^2];
        }

        else if (sizeStr.EndsWith("MB"))
        {
            multiplier = 1024L * 1024;
            sizeStr = sizeStr[..^2];
        }

        else if (sizeStr.EndsWith("KB"))
        {
            multiplier = 1024L;
            sizeStr = sizeStr[..^2];
        }

        if (long.TryParse(sizeStr, out long size))
        {
            sizeInBytes = size * multiplier;
            return true;
        }

        return false;
    }
}
