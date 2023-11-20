using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Reflection;
using TaxCalculatorBlazor;
using TaxCalculatorBlazor.Services;
using TaxCalculatorLibary.Models;

Thread.Sleep(1000);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7169") });

builder.Services.AddScoped<IMainService, MainService>();

await builder.Build().RunAsync();

