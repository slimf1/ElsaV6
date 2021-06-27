using System;

namespace ElsaV6.Utils
{
    public class Logger
    {
        public static bool Timestamp { get; set; } = true;
        public static bool Enabled { get; set; } = true;

        private static void ColoredLog(string message, ConsoleColor color)
        {
            if (!Enabled) return;
            Console.ForegroundColor = color;
            Console.WriteLine("{0}{1}", Timestamp ? $"[{DateTime.Now}] " : "", message);
            Console.ResetColor();
        }

        public static void Info(string message)
        {
            ColoredLog(message, ConsoleColor.Blue);
        }

        public static void Message(string message)
        {
            ColoredLog(message, ConsoleColor.White);
        }

        public static void Error(string message)
        {
            ColoredLog(message, ConsoleColor.Red);
        }

        public static void Debug(string message)
        {
            ColoredLog(message, ConsoleColor.Yellow);
        }
    }
}
