using System.Collections.Generic;

namespace BulgarianWines.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IDeletableEntityRepository<ApplicationRole> rolesRepository)
        {
            this.usersRepository = usersRepository;
            this.rolesRepository = rolesRepository;
        }

        public T GetById<T>(string id) => this.usersRepository
            .AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

        public async Task<bool> DeleteAsync(string id)
        {
            var user = this.GetById(id);

            if (user == null)
            {
                return false;
            }

            this.usersRepository.Delete(user);

            //foreach (var role in user.Roles)
            //{
            //    this.rolesRepository.Delete(role);
            //}

            //await this.rolesRepository.SaveChangesAsync();
            await this.usersRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreAsync(string id)
        {
            var user = this.GetDeletedUserById(id);

            if (user == null)
            {
                return false;
            }

            this.usersRepository.Undelete(user);
            await this.usersRepository.SaveChangesAsync();

            return true;
        }

        public IEnumerable<T> GetAllDeleted<T>() => this.usersRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .To<T>()
            .ToList();

        // public string GetImage() => this.usersRepository
        //    .AllAsNoTracking()
        //    .Select(x => x.ImageUrl)
        //    .FirstOrDefault();

        private ApplicationUser GetById(string id) =>
            this.usersRepository.All().Include(x => x.Roles)
                .FirstOrDefault(x => x.Id == id);

        private ApplicationUser GetDeletedUserById(string id) =>
            this.usersRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefault(x => x.IsDeleted && x.Id == id);
    }
}
