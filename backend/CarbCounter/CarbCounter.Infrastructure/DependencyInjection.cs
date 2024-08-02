using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using CarbCounter.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CarbCounter.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer(configuration.GetConnectionString("sql-server"));

        services.AddIdentity();

        return services;
    }

    private static IServiceCollection AddSqlServer(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("SQL connection string does not exist.");
        }

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString: ""));

        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}