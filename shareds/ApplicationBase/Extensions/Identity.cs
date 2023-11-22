using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;

namespace ApplicationBase.Extensions;

public static class Identity
{
    public static void AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi()
            .AddMicrosoftGraph(configuration.GetSection("DownstreamApi"))
            .AddInMemoryTokenCaches();
    }

    public static void UseApplicationIdentity(this IApplicationBuilder app)
    {
        app.UseAuthentication();

        app.UseAuthorization();
    }
}