using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaxCalculatorAPI.Data;
using TaxCalculatorAPI.Extentions;
using TaxCalculatorAPI.Repository;
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


        // Add services to the container.

        builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IMainControllerService, MainControllerService>();
        builder.Services.AddScoped<IDataBaseRepository, DataBaseRespository>();
        builder.Services.AddScoped<IFileRepository, FileRepository>();
        builder.Services.AddScoped<ISocialSecurityService, SocialSecurityService>();
        builder.Services.AddScoped<ITaxInformationService, TaxInformationService>();
        builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        builder.Services.AddScoped<IAccountService, AccountService>();
        builder.Services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false; // Set to false to exclude special characters
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        }
        ).AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();
        builder.Services.AddAuthentication();

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

        if (File.Exists(Path.Combine(dataDirectory, "Data", "database.db")))
        {
            File.Delete(Path.Combine(dataDirectory, "Data", "database.db"));
        }

        SocialSecurityService socialSecurityService = new(new FileRepository(), new DataBaseRespository(new ApplicationDBContext()));
        TaxInformationService taxInformationService = new(new FileRepository(), new DataBaseRespository(new()));
        app.ApplyMigrations();

        socialSecurityService.MigrateDataFromJsonToDataBase(dataDirectory).Wait();
        taxInformationService.MigrateDataFromJsonToDataBase(dataDirectory).Wait();


        Tracking.DataPathInit(dataDirectory);

        app.Run();

    }
}