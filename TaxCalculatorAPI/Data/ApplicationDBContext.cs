using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext()
        {

        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<SocialSecurityRates> SocialSecurityRates { get; set; }
        public DbSet<TaxInformation> TaxInformations { get; set; }
        public DbSet<TaxInformationStep> TaxInformationSteps { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure your database provider here
                optionsBuilder.UseSqlite("Data Source=Data/database.db");
            }
        }

    }
}
