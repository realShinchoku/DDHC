using EasyCaching.Core.Configurations;
using EasyCaching.InMemory;
using EFCoreSecondLevelCacheInterceptor;
using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;

namespace StudentService.Extensions;

/// <summary>
///     Cấu hình caching service
/// </summary>
public static class CachingServiceExtensions
{
    /// <summary>
    ///     Cấu hình caching service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddCachingService(this IServiceCollection services,
        IConfiguration configuration)
    {
        var cacheOption = configuration.GetSection("CacheOption").Value;

        services.AddEasyCaching(option =>
        {
            switch (cacheOption)
            {
                case "Redis":
                {
                    var serializerName = configuration.GetSection("Redis:SerializerName").Value;
                    var serverEndPoint = new ServerEndPoint(configuration.GetSection("Redis:Host").Value,
                        int.Parse(configuration.GetSection("Redis:Port").Value!));
                    option.UseRedis(config =>
                    {
                        config.DBConfig.AllowAdmin = true;
                        config.DBConfig.SyncTimeout = 10000;
                        config.DBConfig.AsyncTimeout = 10000;
                        config.DBConfig.Endpoints.Add(serverEndPoint);
                        config.EnableLogging = true;
                        config.SerializerName = serializerName;
                        config.DBConfig.ConnectionTimeout = 10000;
                    }, cacheOption);

                    option.WithMessagePack(so =>
                    {
                        so.EnableCustomResolver = true;
                        so.CustomResolvers = CompositeResolver.Create(
                            new IMessagePackFormatter[]
                            {
                                DbNullFormatter.Instance // This is necessary for the null values
                            },
                            new IFormatterResolver[]
                            {
                                NativeDateTimeResolver.Instance,
                                ContractlessStandardResolver.Instance,
                                StandardResolverAllowPrivate.Instance,
                                TypelessContractlessStandardResolver.Instance,
                                DynamicGenericResolver.Instance
                            });
                    }, serializerName);
                    break;
                }
                case "InMemory":
                    option.UseInMemory(config =>
                    {
                        config.DBConfig = new InMemoryCachingOptions
                        {
                            ExpirationScanFrequency = 60,
                            SizeLimit = 100,
                            EnableReadDeepClone = false,
                            EnableWriteDeepClone = false
                        };
                        config.MaxRdSecond = 120;
                        config.EnableLogging = true;
                        config.LockMs = 5000;
                        config.SleepMs = 300;
                    }, cacheOption);
                    break;
            }
        });

        var cachedTimeSpan = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production
            ? TimeSpan.FromMinutes(90)
            : TimeSpan.FromMinutes(5);

        services.AddEFSecondLevelCache(options =>
            options
                .UseEasyCachingCoreProvider(cacheOption!)
                .DisableLogging()
                .CacheAllQueriesExceptContainingTableNames(CacheExpirationMode.Sliding, cachedTimeSpan,
                    "__EFMigrationsHistory", "InboxState", "OutboxState", "OutboxMessage")
                .SkipCachingCommands(cmdText => cmdText.Contains("EXISTS"))
        );
    }

    /// <inheritdoc />
    private class DbNullFormatter : IMessagePackFormatter<DBNull>
    {
        public static readonly DbNullFormatter Instance = new();

        private DbNullFormatter()
        {
        }

        /// <inheritdoc />
        public void Serialize(ref MessagePackWriter writer, DBNull value, MessagePackSerializerOptions options)
        {
            writer.WriteNil();
        }

        /// <inheritdoc />
        public DBNull Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            return DBNull.Value;
        }
    }
}