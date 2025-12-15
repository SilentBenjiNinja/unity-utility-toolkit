using System;
using System.Runtime.CompilerServices;

namespace bnj.utility_toolkit.Runtime.Logging
{
    // TODO:
    // make stacktrace better! -> compile and add as dll so this won't show in stacktrace!
    // log to screen option?
    public static class LogUtils
    {
        static Action<string> LogFunction(LogLevel logLevel = LogLevel.Debug) => logLevel switch
        {
            LogLevel.Debug => UnityEngine.Debug.Log,
            LogLevel.Info => UnityEngine.Debug.Log,
            LogLevel.Warn => UnityEngine.Debug.LogWarning,
            LogLevel.Error => UnityEngine.Debug.LogError,
            LogLevel.Fatal => UnityEngine.Debug.LogError,
            _ => throw new NotImplementedException(),
        };

        static string ColorString(LogLevel logLevel = LogLevel.Debug) => logLevel switch
        {
            LogLevel.Debug => "008E6E",
            LogLevel.Info => "57C310",
            LogLevel.Warn => "FBC227",
            LogLevel.Error => "F62424",
            LogLevel.Fatal => "A60000",
            _ => throw new NotImplementedException(),
        };

        static string AsFormattedMessage(object message, string member, string file, int line, LogLevel logLevel, string context)
        {
            var logLevelSuffix = $"[{logLevel.ToString().ToUpper()}]";
            var contextSuffix = context == "" ? "" : $" <{context}>";
            var trace = $"{System.IO.Path.GetFileName(file)}:{line}";
#if UNITY_EDITOR
            logLevelSuffix = $"<color=#{ColorString(logLevel)}><b>{logLevelSuffix}</b></color>";
            contextSuffix = context == "" ? "" : $"\n<i><{context}></i>";
            trace = $"<a href=\"{file}\" line=\"{line}\">{trace}</a>";
#endif
            return $"{message}{contextSuffix}\n{logLevelSuffix} {member} ({trace})";
        }

        public static void Debug(object message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            string context = "") =>
            LogFunction(LogLevel.Debug)(AsFormattedMessage(message, member, file, line, LogLevel.Debug, context));

        public static void Info(object message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            string context = "") =>
            LogFunction(LogLevel.Info)(AsFormattedMessage(message, member, file, line, LogLevel.Info, context));

        public static void Warn(object message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            string context = "") =>
            LogFunction(LogLevel.Warn)(AsFormattedMessage(message, member, file, line, LogLevel.Warn, context));

        public static void Error(object message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            string context = "") =>
            LogFunction(LogLevel.Error)(AsFormattedMessage(message, member, file, line, LogLevel.Error, context));

        public static void Fatal(object message,
            [CallerMemberName] string member = "",
            [CallerFilePath] string file = "",
            [CallerLineNumber] int line = 0,
            string context = "") =>
            LogFunction(LogLevel.Fatal)(AsFormattedMessage(message, member, file, line, LogLevel.Fatal, context));

        // https://stackoverflow.com/questions/2031163/when-to-use-the-different-log-levels
        enum LogLevel
        {
            Debug = 0,
            Info = 1,
            Warn = 2,
            Error = 4,
            Fatal = 5,
        }
    }
}
