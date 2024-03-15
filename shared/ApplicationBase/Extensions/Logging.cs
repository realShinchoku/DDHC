using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ApplicationBase.Extensions;

public static class Logging
{
    public static void AddLoggingService(this ILoggingBuilder logging, IConfiguration configuration)
    {
        logging.ClearProviders();
        var logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(configuration["Logging:FilePath"] ?? "Logs/Log_.log", rollingInterval: RollingInterval.Hour);
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") != Environments.Production)
            logger.MinimumLevel.Information();
        else
            logger.MinimumLevel.Warning();
        logging.AddSerilog(logger.CreateLogger());
    }
}