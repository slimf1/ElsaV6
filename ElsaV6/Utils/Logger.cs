using System;

namespace ElsaV6.Utils
{
    public class Logger
    {
        public bool Timestamp { get; set; }
        public bool Enabled { get; set; }

        public Logger()
        {
            Timestamp = true;
            Enabled = true;
        }

        private void ColoredLog(string message, ConsoleColor color)
        {
            if (!Enabled) return;
            Console.ForegroundColor = color;
            Console.WriteLine("{0}{1}", Timestamp ? $"[{DateTime.Now}] " : "", message);
            Console.ResetColor();
        }

        public void Info(string message)
        {
            ColoredLog(message, ConsoleColor.Blue);
        }

        public void Message(string message)
        {
            ColoredLog(message, ConsoleColor.White);
        }

        public void Error(string message)
        {
            ColoredLog(message, ConsoleColor.Red);
        }

        public void Debug(string message)
        {
            ColoredLog(message, ConsoleColor.Yellow);
        }
    }
}
