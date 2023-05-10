using user_app.Repositories.Implementations;
using user_app.Repositories.Interfaces;
using user_app.Services.Implementation;
using user_app.Services.Interfaces;

namespace user_app.Extensions;

public static class ServicesExtension
{
    public static void AddDomain(this IServiceCollection services)
    {
        AddDomainServices(services);
        AddRepositories(services);
    }

    private static void AddDomainServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserGroupRepository, UserGroupRepository>();
        services.AddScoped<IUserStateRepository, UserStateRepository>();
    }

    public static void AddHelpers(this IServiceCollection services)
    {
        services.AddSingleton<IAuthService, AuthorizationService>();
    }
}