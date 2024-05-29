using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repository.Database;
using Repository.Interfaces;

namespace Repository.Extensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PersonRegistrationContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    public static IServiceCollection RegisterRepositoryServiceCollection(this IServiceCollection services)
    {
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IResidenceRepository, ResidenceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}