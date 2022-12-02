using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Services.Sessions;
using Services.Users;
using Services.PricePlans;
using Services.Payments;
using Services.Subscriptions;
using Services.TurnCards;
using Services.Workouts;
using Squads.Shared.Auth;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;
using Squads.Shared.PricePlans;
using Squads.Shared.Payments;
using Squads.Shared.Subscriptions;
using Squads.Shared.TurnCards;

namespace Squads.Server.Extensions;

public static class DbServiceExtensions
{
    public static IServiceCollection AddDbContextAndServices<TContext>(
        this IServiceCollection services,
        IConfiguration config,
        IWebHostEnvironment env
    ) where TContext : DbContext
    {
        services.AddEntityFrameworkNpgsql().AddDbContext<TContext>(options =>
        {
            options
                .UseNpgsql(config.GetConnectionString("SquadsSqlDatabase"))
                .EnableSensitiveDataLogging(config.GetValue<bool>("Logging:EnableSqlParameterLogging"));

            if (env.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        }
        );

        return services;
    }

    public static IServiceCollection AddAllDbServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPricePlanService, PricePlanService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<ITurnCardService, TurnCardService>();
        return services;
    }

    public static IServiceCollection AddDbSeedServices(this IServiceCollection services)
    {
        services.AddScoped<SquadDataInitializer>();
        return services;
    }
}
