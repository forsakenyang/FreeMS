namespace FreeMS;

using NLog;
using NLog.Config;
using NLog.Targets;

class Program
{
    public static void Main(string[] args)
    {
        setupLogs();
        var server = new ChannelServer();
        server.Startup().AsTask().Wait();
        Console.ReadKey();
    }

    private static void setupLogs()
    {
        var config = new LoggingConfiguration();
        var fileTarget = new FileTarget { FileName = "logs/channel.txt" };
        var consoleTarget = new ColoredConsoleTarget();
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
        config.AddRule(LogLevel.Debug, LogLevel.Fatal, consoleTarget);
        LogManager.Configuration = config;
    }
}