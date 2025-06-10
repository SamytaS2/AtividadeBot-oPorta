using System.Collections.Generic;

public static class ReplayData
{
    public static List<(int, int)> SwapHistory = new List<(int, int)>();
    public static bool ShouldPlayReplay = false;
}
