using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Organization.Models.Authentication;
using Organization.Models.Contexts;
using Swashbuckle.AspNetCore.Swagger;
using TFlic;

namespace Organization;

public static class Program
{
    /// <summary>
    /// Конфигурация запуска приложения
    /// </summary>
    public static IConfiguration? AppConfiguration;
    /*
     * пришлось вынести конфигурацию в поле класса Program,
     * так как не нашел другого способа обращаться к файлам
     * appsettings.json / appsettings.Development.json в
     * зависимости от переменной окружения 
     */
    
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        AppConfiguration = builder.Configuration;
        // Add services to the container.

        builder.Services.AddControllers().AddNewtonsoftJson();
        
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = AuthenticationManager.AccessTokenValidationParameters;
                }
            );
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
            {
                options.DocumentFilter<JsonPatchDocumentFilter>();
#if AUTH
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
                                  "Enter your access token in the text input below.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme,
                            },
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
#endif
            }
        );

        // Add DbContexts to static aggregator
        var dbConnectionString = AppConfiguration.GetConnectionString(Constants.DbTarget)!;
        DbContexts.DbConnectionString = dbConnectionString;

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

