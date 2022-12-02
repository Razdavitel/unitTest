using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Auth;
using Services.Mails;
using Services.PricePlans;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace Squads.Server.Extensions;

public static class ServiceExtensions { 
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService, TokenService>();
        services.AddScoped<MailService, MailService>();
        services.AddScoped<PricePlanService, PricePlanService>();
        return services;
    }
    public static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(options => {
            options.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        return services;
    }
    // extract token auth of header
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        return services;
    }
    public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program).Assembly);
        return services;
    }
}
