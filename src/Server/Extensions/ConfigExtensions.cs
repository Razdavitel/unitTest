using Squads.Shared.Config;

namespace Squads.Server.Extensions;

public static class ConfigExtensions
{
    public static IServiceCollection LoadConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton(config.GetSection("MailSettings").Get<MailSettings>());
        return services;
    }
}