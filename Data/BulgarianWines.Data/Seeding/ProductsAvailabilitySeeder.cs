namespace BulgarianWines.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public class ProductsAvailabilitySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Availabilities.Any())
            {
                var productAvailabilities = new List<string>
                {
                    "In Stock",
                    "Out of Stock",
                };

                foreach (var productAvailability in productAvailabilities)
                {
                    await dbContext.Availabilities.AddAsync(new Availability
                    {
                        Status = productAvailability,
                    });
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
