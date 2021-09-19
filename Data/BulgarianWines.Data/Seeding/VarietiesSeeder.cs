namespace BulgarianWines.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public class VarietiesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Varieties.Any())
            {
                return;
            }

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Cabernet Franc",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Cabernet Sauvignon",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Carménère",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Chardonnay",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Gewürztraminer",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Grenache",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Malbec",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Merlot",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Muscat Ottonel",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Nebbiolo",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Pinotage",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Pinot Grigio",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Pinot NoirL",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Riesling",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Sauvignon Blanc",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Sémillon",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Syrah",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Tempranillo",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Viognier",
            });

            await dbContext.Varieties.AddAsync(new Variety
            {
                Name = "Zinfandel",
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
