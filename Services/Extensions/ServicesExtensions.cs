using Microsoft.Extensions.DependencyInjection;
using Services.Interfaces;

namespace Services.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection RegisterServicesServiceCollection(this IServiceCollection services)
    {
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IResidenceService, ResidenceService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}