using Microsoft.EntityFrameworkCore;
using TaxCalculatorLibary.Models;

namespace TaxCalculatorAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {

        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<SocialSecurityRates> SocialSecurityRates { get; set; }
        public DbSet<TaxInformation> TaxInformation { get; set; }
        public DbSet<TaxInformationStep> TaxInformationStep { get; set; }

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
