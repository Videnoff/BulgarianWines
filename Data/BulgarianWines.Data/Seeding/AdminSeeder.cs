namespace BulgarianWines.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class AdminSeeder : ISeeder
    {
        private readonly AdminCredentials adminCredentials;

        public AdminSeeder(AdminCredentials adminCredentials)
        {
            this.adminCredentials = adminCredentials;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await this.SeedRoleAsync(userManager, this.adminCredentials);
        }

        private async Task SeedRoleAsync(UserManager<ApplicationUser> userManager, AdminCredentials adminCredentials)
        {
            if (userManager.FindByEmailAsync(this.adminCredentials.AdminEmail).Result == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = this.adminCredentials.AdminUsername,
                    Email = this.adminCredentials.AdminEmail,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(admin, this.adminCredentials.AdminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, GlobalConstants.AdministratorRoleName);
                }
            }
        }
    }
}
