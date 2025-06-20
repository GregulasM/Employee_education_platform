using System.Reflection;
using eep_backend;
using Microsoft.OpenApi.Models;

namespace Employee_education_platform;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        builder.Services.AddControllers();
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Employee education platform API",
                Description = "Методы пралтформы EEP и их описание.",
            });

            // using System.Reflection;
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        app.UseAuthorization();

        // var summaries = new[]
        // {
        //     "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        // };

        // app.MapGet("/weatherforecast", (HttpContext httpContext) =>
        //     {
        //         var forecast = Enumerable.Range(1, 5).Select(index =>
        //                 new WeatherForecast
        //                 {
        //                     Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //                     TemperatureC = Random.Shared.Next(-20, 55),
        //                     Summary = summaries[Random.Shared.Next(summaries.Length)]
        //                 })
        //             .ToArray();
        //         return forecast;
        //     })
        //     .WithName("GetWeatherForecast");

        app.Run();
    }
}