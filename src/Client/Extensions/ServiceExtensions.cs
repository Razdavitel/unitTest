using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Squads.Client.Auth;
using Squads.Client.Sessions;
using Squads.Client.Users;
using Squads.Client.Workouts;
using Squads.Client.PricePlans;
using Squads.Client.Payments;
using Squads.Shared.Auth;
using Squads.Shared.Sessions;
using Squads.Shared.Users;
using Squads.Shared.Workouts;
using Squads.Shared.PricePlans;
using Squads.Shared.Payments;

namespace Squads.Client.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IWorkoutService, WorkoutService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPricePlanService, PricePlanService>();
        services.AddScoped<PricePlanState, PricePlanState>();
        services.AddScoped<IPaymentService, PaymentService>();
        return services;
    }

    public static IServiceCollection AddSquadHttpClient(this IServiceCollection services, IWebAssemblyHostEnvironment hostEnv) {
        services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(hostEnv.BaseAddress) });
        return services;
    }
}
