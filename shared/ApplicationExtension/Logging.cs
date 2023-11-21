using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ApplicationExtension;

public static class Logging
{
    public static void AddLoggingService(this ILoggingBuilder logging, IConfiguration configuration)
    {
        logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(configuration["Logging:FilePath"] ?? "Logs/Log_.log", rollingInterval: RollingInterval.Hour);
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development)
            logger.MinimumLevel.Debug();
        else
            logger.MinimumLevel.Information();
        logging.AddSerilog(logger.CreateLogger());
    }
}