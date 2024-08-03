using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using CarbCounter.Core.Common.Interfaces;
using CarbCounter.Core.Services;

namespace CarbCounter.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<IJsonWebTokenService, JsonWebTokenService>();

        return services;
    }
}