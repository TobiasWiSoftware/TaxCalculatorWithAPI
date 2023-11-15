using TaxCalculatorASP;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using TaxCalculatorAPI.Services;

internal class Program
{
    public Program(IWebHostEnvironment env)
    {
    }

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        string _pathSocialSecurityRatesJson = null;
        string _pathTaxInformationJson = null;

        string dataDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location.Substring(0, Assembly.GetEntryAssembly().Location.IndexOf("bin\\")));



        _pathSocialSecurityRatesJson = Path.Combine(dataDirectory, "Data", "SocialSecurityRates.json");
        _pathTaxInformationJson = Path.Combine(dataDirectory, "Data", "TaxInformation.json");




        using (StreamReader r = new(_pathSocialSecurityRatesJson))
        {
            string s = r.ReadToEnd();
            SocialSecurityRates.SetList(JsonConvert.DeserializeObject<List<SocialSecurityRates>>(s));
        }

        using (StreamReader r = new(_pathTaxInformationJson))
        {
            string s = r.ReadToEnd();
            TaxInformation.SetList(JsonConvert.DeserializeObject<List<TaxInformation>>(s));
        }
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IMainService, MainService>();

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