using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaxCalculatorAPI.Services;
using TaxCalculatorLibary.Models;

internal class Program
{
    public Program(IWebHostEnvironment env)
    {
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string dataDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));

        SocialSecurityRates.LoadDataFromJson(dataDirectory);

        TaxInformation.LoadDataFromJson(dataDirectory);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IMainService, MainService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("https://localhost:44319")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
        });

        var app = builder.Build();
        app.UseCors("AllowSpecificOrigin");

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