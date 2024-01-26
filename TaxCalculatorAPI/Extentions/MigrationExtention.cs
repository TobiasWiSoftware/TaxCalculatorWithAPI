using Microsoft.EntityFrameworkCore;
using System;
using TaxCalculatorAPI.Data;

namespace TaxCalculatorAPI.Extentions
{
    public static class MigrationExtention
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationDBContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            dbContext.Database.Migrate();
        }
    }
}
