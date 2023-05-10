using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using user_app.Context;
using user_app.Extensions;
using user_app.Middlewares;

namespace user_app;
public class Program
{
    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        });

        services.AddControllers();
        services.AddSwaggerGen(option =>
        {
            option.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
            {
                Description = "Header для тестового задания: Basic YWRtaW46MTIz" +
                              "<br/>Впишите его в поле и нажмите 'Authorize'" +
                              "<br/>YWRtaW46MTIz это Base64 code для админ пользователя 'admin:123'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Basic"
            });

            var apiSecurityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Basic"
                },
                Name = "Basic",
                In = ParameterLocation.Header,
            };
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                [apiSecurityScheme] = new List<string>()
            });
        });
        services.AddEndpointsApiExplorer();
        services.AddDomain();
        services.AddHelpers();
    }

    private static void ConfigureApplication(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseMiddleware<ErrorsHandlerMiddleware>();
        app.UseMiddleware<BasicAuthMiddleware>();
        app.MapControllers();
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        if (context.Database.GetPendingMigrations().Any()) {
            context.Database.Migrate();
        }
        scope.ServiceProvider.GetService<ApplicationContext>();
    }

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services, builder.Configuration);
        var app = builder.Build();
        ConfigureApplication(app);
        app.Run();
    }
}