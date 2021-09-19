namespace BulgarianWines.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public class HarvestsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Harvests.Any())
            {
                return;
            }

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2021",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2020",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2019",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2018",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2017",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2016",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2015",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2014",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2013",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2012",
            });

            await dbContext.Harvests.AddAsync(new Harvest
            {
                Year = "2011",
            });

            await dbContext.SaveChangesAsync();
        }
    }
}
