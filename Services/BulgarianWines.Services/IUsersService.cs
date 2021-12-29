namespace BulgarianWines.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        public T GetById<T>(string id);

        // public string GetImage();

        public Task<bool> DeleteUserAsync(string id);

        public Task<bool> DeleteRoleAsync(string id);

        public Task<bool> RestoreUserAsync(string id);

        public Task<bool> RestoreRoleAsync(string id);

        public IEnumerable<T> GetAllDeletedUsers<T>();

        public IEnumerable<T> GetAllDeletedRoles<T>();
    }
}
