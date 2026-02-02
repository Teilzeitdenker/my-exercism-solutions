using System;

static class LogLine
{
    public static string Message(string logLine)
    {
        string result = "";
        if (logLine.Contains(":")) {
            int startIndex = logLine.IndexOf(":", 0) + 1;
            result = logLine.Substring(startIndex).Trim();
        }
        return result;
    }

    public static string LogLevel(string logLine)
    {
        string result = "";
        if (logLine.Contains("[") && logLine.Contains("]")) {
            int startIndex = logLine.IndexOf("[", 0) + 1;
            int endIndex = logLine.IndexOf("]", startIndex);
            result = logLine.Substring(startIndex, endIndex - startIndex).Trim().ToLower();
        }
    return result;
    }

    public static string Reformat(string logLine)
    {
        return Message(logLine) + " (" + LogLevel(logLine) + ")"; 
    }
}
