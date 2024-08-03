using CarbCounter.Application.Account.Commands.Authenticate;
using CarbCounter.Application.Account.Commands.Create;
using CarbCounter.Application.Account.Dto;
using CarbCounter.Application.Common.Requests;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace CarbCounter.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRequestHandlers();
        
        return services;
    }

    private static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        services.AddScoped<CreateAccountCommandHandler>();
        services.AddScoped<IRequestHandler<CreateAccountCommand, string>, CreateAccountCommandHandler>();
        
        services.AddScoped<AuthenticateAccountCommandHandler>();
        services.AddScoped<IRequestHandler<AuthenticateAccountCommand, AuthenticateAccountDto>, AuthenticateAccountCommandHandler>();
        
        return services;
    }
}
