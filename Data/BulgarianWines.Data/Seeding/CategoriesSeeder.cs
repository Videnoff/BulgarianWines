using System;
using System.Linq;
using System.Threading.Tasks;
using BulgarianWines.Data.Models;

namespace BulgarianWines.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Red Wines",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "White Wines",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Rose Wines",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Sparkling Wines",
            });
            await dbContext.Categories.AddAsync(new Category
            {
                Name = "Dessert Wines",
            });

            await dbContext.SaveChangesAsync();
        }
    }
}