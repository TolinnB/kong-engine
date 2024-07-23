using System;
using System.IO;

public static class Logger
{
    private static readonly string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug.log");

    public static void Log(string message)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}: {message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logging failed: {ex.Message}");
        }
    }
}
