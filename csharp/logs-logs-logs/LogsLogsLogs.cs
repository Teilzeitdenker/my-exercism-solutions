using System;

enum LogLevel : byte
{
    Unknown = 0,
    Trace = 1,
    Debug = 2,
    Info  = 4, 
    Warning = 5,
    Error = 6,
    Fatal = 42
}

static class LogLine
{
    public static LogLevel ParseLogLevel(string logLine) => logLine.Substring(1, 3) switch
        {
            "TRC" => LogLevel.Trace,
            "DBG" => LogLevel.Debug,
            "INF" => LogLevel.Info,
            "WRN" => LogLevel.Warning,
            "ERR" => LogLevel.Error,
            "FTL" => LogLevel.Fatal,
            _ => LogLevel.Unknown
        };
    

    public static string OutputForShortLog(LogLevel logLevel, string message)
    {
        // Nutze den Trick aus dem C#-Buch, formatiere das LogLevel mit "D", dann wird es als Zahl ausgegeben...
        return string.Format("{0:D}:{1}", logLevel, message);
    }
}
