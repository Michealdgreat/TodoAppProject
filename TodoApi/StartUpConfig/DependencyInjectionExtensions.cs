using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;
using TodoLibrary.DataAccess;

namespace TodoApi.StartUpConfig;

public static class DependencyInjectionExtensions
{
    public static void AddStandardServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
       
    }

    public static void AddCustomServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<ITodoData, TodoData>(); //Transient create an instance everytime while Singleton does not.
    }


    public static void AddHealthCheckServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("Default"));

    }

    public static void AddAuthenticationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(opts =>
        {
            opts.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

        });


        builder.Services.AddAuthentication("bearer").AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                ValidAudience = builder.Configuration.GetValue<string>("Authentication:Audience"),
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("Authentication:SecretKey")))
            };

        });
    }

}
