using System.Collections.Generic;
using static TypeInit;

public class ChunkMap
{
    private static readonly Dictionary<IntVector2, string> Map = new Dictionary<IntVector2, string>();

    public static void SetValue(IntVector2 position, string value)
    {
        Map[position] = value;
    }

    public static string GetValue(IntVector2 position)
    {
        if (Map.TryGetValue(position, out string value))
            return value;
        else
            return "0";
    }

    public static Dictionary<IntVector2, string> GetMap()
    {
        return Map;
    }

    public static void RemoveIndex(IntVector2 position)
    {
        Map.Remove(position);
    }

    public static int GetMapLength()
    {
        return Map.Count;
    }
}
