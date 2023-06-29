using System.Collections.Generic;

public class ChunkMap
{
    private static readonly Dictionary<(int, int), string> Map= new Dictionary<(int, int), string>();

    public static void SetValue(int x, int y, string value)
    {
        Map[(x, y)] = value;
    }

    public static string GetValue(int x, int y)
    {
        if (Map.TryGetValue((x, y), out string value))
            return value;
        else
            return "0";
    }

    public static void RemoveIndex(int x, int y)
    {
        Map.Remove((x, y));
    }
}
