using Newtonsoft.Json;

namespace TaxCalculatorASP
{
    public class Startup    
    {
        private IWebHostEnvironment _env;
        private string _pathSocialSecurityRatesJson;
        private string _pathTaxInformationJson;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddCors(); // Added
            services.AddRazorPages().AddRazorPagesOptions(o => o.Conventions.AddPageRoute("/Start", "")); //Marketplace muste be renamed or deletet to change this site
            _pathSocialSecurityRatesJson = Path.Combine(_env.ContentRootPath, "Data", "SocialSecurityRates.json");
            _pathTaxInformationJson = Path.Combine(_env.ContentRootPath, "Data", "TaxInformation.json");

            using (StreamReader r = new(_pathSocialSecurityRatesJson))
            {
                string s = r.ReadToEnd();
                SocialSecurityRates.SetList(JsonConvert.DeserializeObject<List<SocialSecurityRates>>(s));
            }

            using(StreamReader r = new(_pathTaxInformationJson))
            {
                string s = r.ReadToEnd();
                TaxInformation.SetList(JsonConvert.DeserializeObject<List<TaxInformation>>(s));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseDefaultFiles();

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); // For exces own pc on own core // Fluence syntax

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

        }
    }
}
