using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaxCalculatorAPI.Services;
using TaxCalculatorLibary.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null)
        {
            throw new InvalidOperationException("Entry assembly not found.");
        }

        string entryAssemblyLocation = entryAssembly.Location;
        int binIndex = entryAssemblyLocation.IndexOf("bin\\");

        string dataDirectory;
        if (binIndex >= 0)
        {
            string pathUntilBin = entryAssemblyLocation.Substring(0, binIndex);
            dataDirectory = Path.GetDirectoryName(pathUntilBin) ?? throw new InvalidOperationException("Directory path for 'bin\\' not found.");
        }
        else
        {
            dataDirectory = Path.GetDirectoryName(entryAssemblyLocation) ?? throw new InvalidOperationException("Directory path for entry assembly not found.");
        }

       

        SocialSecurityRates.LoadDataFromJson(dataDirectory);

        TaxInformation.LoadDataFromJson(dataDirectory);

        Tracking.DataPathInit(dataDirectory);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IMainService, MainService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAnyOrigin",
                      builder => builder.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod());
        });

        builder.WebHost.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ListenAnyIP(43721, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                // Sie können hier auch SSL-Optionen konfigurieren, falls benötigt
            });
        });



        var app = builder.Build();
        app.UseCors("AllowAnyOrigin");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection(); //dont need because api is not exposed to the internet

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}