using API.Extensions;
using Repository.Extensions;
using Serilog;
using Services.Extensions;

namespace API;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console(
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .CreateLogger();

        Log.Information("Initializing...");

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddDatabase(builder.Configuration);
        builder.Services.RegisterRepositoryServiceCollection();

        builder.Services.RegisterServicesServiceCollection();

        builder.Services.AddJwtAuthServiceCollection(builder.Configuration);
        builder.Services.AddSwaggerServiceCollection();

        Log.Information("Adding services...");
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        Log.Information("Building app...");
        var app = builder.Build();

        Log.Information("Adding middleware...");
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthorization();


        app.MapControllers();

        Log.Information("Starting the app!");
        app.Run();
    }
}