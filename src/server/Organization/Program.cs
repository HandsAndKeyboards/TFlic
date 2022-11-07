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

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

