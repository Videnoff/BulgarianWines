namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public interface IUserMessagesService
    {
        public IEnumerable<T> All<T>();

        public IEnumerable<T> AllDeleted<T>();

        public IEnumerable<T> GetUnreadMessages<T>();

        public Task CreateAsync<T>(T model);

        public Task<bool> DeleteAsync(string id);

        public Task<bool> RestoreAsync(string id);

        public Task<bool> SetToReadAsync(string id, bool isRead);

        public T GetById<T>(string id);
    }
}
