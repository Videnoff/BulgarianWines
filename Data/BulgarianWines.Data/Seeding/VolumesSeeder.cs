namespace BulgarianWines.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public class VolumesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Volumes.Any())
            {
                return;
            }

            await dbContext.Volumes.AddAsync(new Volume
            {
                Quantity = "0.25L",
            });

            await dbContext.Volumes.AddAsync(new Volume
            {
                Quantity = "0.5L",
            });

            await dbContext.Volumes.AddAsync(new Volume
            {
                Quantity = "0.7L",
            });

            await dbContext.Volumes.AddAsync(new Volume
            {
                Quantity = "0.75L",
            });

            await dbContext.Volumes.AddAsync(new Volume
            {
                Quantity = "1L",
            });

            await dbContext.Volumes.AddAsync(new Volume
            {
                Quantity = "1.5L",
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
