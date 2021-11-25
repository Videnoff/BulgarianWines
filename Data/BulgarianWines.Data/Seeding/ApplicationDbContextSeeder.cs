namespace BulgarianWines.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ApplicationDbContextSeeder : ISeeder
    {
        private readonly AdminCredentials adminCredentials;
        private readonly SuperAdminCredentials superAdminCredentials;

        public ApplicationDbContextSeeder(
            AdminCredentials adminCredentials,
            SuperAdminCredentials superAdminCredentials)
        {
            this.adminCredentials = adminCredentials;
            this.superAdminCredentials = superAdminCredentials;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            var seeders = new List<ISeeder>
                          {
                              new RolesSeeder(),
                              new SettingsSeeder(),
                              new CategoriesSeeder(),
                              new VolumesSeeder(),
                              new HarvestsSeeder(),
                              new VarietiesSeeder(),
                              new OriginsSeeder(),
                              new ProductsAvailabilitySeeder(),
                              new AdminSeeder(this.adminCredentials),
                              new SuperAdminSeeder(this.superAdminCredentials),
                          };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
