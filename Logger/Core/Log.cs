using ImageToPDF.Logger.Entities;
using System;

namespace ImageToPDF.Logger.Core
{
    public static class Log
    {
        public static void WriteLine(string message, LoggerEvents loggerEvent = LoggerEvents.Information)
        {
            switch (loggerEvent)
            {
                case LoggerEvents.Information:
                    Console.WriteLine($"{GetDateTimeNowFormated()} {message}");
                    break;
                case LoggerEvents.Warning:
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{GetDateTimeNowFormated()} {message}");
                        Console.ResetColor();
                    }
                    break;
                case LoggerEvents.Error:
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{GetDateTimeNowFormated()} {message}");
                        Console.ResetColor();
                    }
                    break;
                case LoggerEvents.Success:
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{GetDateTimeNowFormated()} {message}");
                        Console.ResetColor();
                    }
                    break;
            }
        }

        private static string GetDateTimeNowFormated()
        {
            DateTime dt = DateTime.Now;
            return $"[{dt.ToShortDateString()} {dt.ToShortTimeString()}]";
        }
    }
}