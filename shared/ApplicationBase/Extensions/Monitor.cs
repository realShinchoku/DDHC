using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Prometheus.SystemMetrics;

namespace ApplicationBase.Extensions;

public static class Monitor
{
    public static void AddMonitor(this IServiceCollection services)
    {
        services.UseHttpClientMetrics();
        services.AddHealthChecks()
            .ForwardToPrometheus();
        services.AddSystemMetrics();
    }

    public static void UseMonitor(this IApplicationBuilder app)
    {
        app.UseMetricServer();
        app.UseHttpMetrics();
        app.UseHttpMetrics(options => { options.ReduceStatusCodeCardinality(); });
    }
}