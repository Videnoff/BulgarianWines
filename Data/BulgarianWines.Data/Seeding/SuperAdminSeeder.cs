namespace BulgarianWines.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    using BulgarianWines.Common;
    using BulgarianWines.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class SuperAdminSeeder : ISeeder
    {
        private readonly SuperAdminCredentials superAdminCredentials;

        public SuperAdminSeeder(SuperAdminCredentials superAdminCredentials)
        {
            this.superAdminCredentials = superAdminCredentials;
        }

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await this.SeedRoleAsync(userManager, this.superAdminCredentials);
        }

        private async Task SeedRoleAsync(UserManager<ApplicationUser> userManager, SuperAdminCredentials superAdminCredentials)
        {
            if (userManager.FindByNameAsync(this.superAdminCredentials.SuperAdminUsername).Result == null)
            {
                var superadmin = new ApplicationUser
                {
                    UserName = this.superAdminCredentials.SuperAdminUsername,
                    Email = this.superAdminCredentials.SuperAdminUsername,
                    EmailConfirmed = true,
                };

                var result = await userManager.CreateAsync(superadmin, this.superAdminCredentials.SuperAdminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(superadmin, GlobalConstants.SuperAdministratorRoleName);
                    await userManager.AddToRoleAsync(superadmin, GlobalConstants.AdministratorRoleName);
                }
            }
        }
    }
}
