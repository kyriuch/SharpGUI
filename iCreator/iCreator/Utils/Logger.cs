using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace iCreator.Utils
{
    public interface ILogger
    {
        void Info(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void Debug(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void Error(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void Warning(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
        void Exception(Exception ex, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0);
    }

    public sealed class Logger : ILogger
    {
        public void Info(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0) =>
            Log(ConsoleColor.Blue, "Info", message, filePath, lineNumber);
        public void Debug(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0) =>
            Log(ConsoleColor.Gray, "Debug", message, filePath, lineNumber);
        public void Error(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0) =>
            Log(ConsoleColor.Red, "Error", message, filePath, lineNumber);
        public void Warning(string message, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0) =>
            Log(ConsoleColor.Yellow, "Warning", message, filePath, lineNumber);
        public void Exception(Exception ex, [CallerFilePath] string filePath = null, [CallerLineNumber] int lineNumber = 0) =>
            Log(ConsoleColor.Magenta, "Exception", $"An unhandled exception has been thrown: { ex.Message }\n\n{ ex.StackTrace }\n", filePath, lineNumber);

        private void Log(ConsoleColor color, string level, string message, string filePath = null, int lineNumber = 0)
        {
            Console.ForegroundColor = color;
            Console.Write($"[{ level }][{ Path.GetFileName(filePath) }:{ lineNumber }]: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }
    }
}
