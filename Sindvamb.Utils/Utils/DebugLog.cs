namespace Sindvamb.Utils.Utils;

public class DebugLog
{
    internal static bool ConsoleWriteLine = false;

    public static void Log(string LogStr, string tag1 = "")
    {
        if (ConsoleWriteLine) Log($"时间 {DateTime.UtcNow.ToString()} :  {LogStr}  , Tag {tag1} ");
    }
}
