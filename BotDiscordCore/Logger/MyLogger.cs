using Discord;

namespace BotDiscordCore.Logger
{
    public class MyLogger
    {
        public static void Line(LogLevel level = LogLevel.Info)
        {
            string prefix = GetLogLevelPrefix(level);
            Console.WriteLine($"------------------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static void Log(string message, LogLevel level = LogLevel.Info)
        {
            string prefix = GetLogLevelPrefix(level);
            Console.WriteLine($"[{DateTime.Now} {prefix}] {message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static string GetLogLevelPrefix(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    return "[INFO]";
                case LogLevel.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    return "[WARNING]";
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    return "[ERROR]";
                case LogLevel.Debug:
                    Console.ForegroundColor = ConsoleColor.Green;
                    return "[DEBUG]";
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return "[UNKNOWN]";

            }
        }
    }
}
