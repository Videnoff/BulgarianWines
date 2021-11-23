namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public interface IUserMessagesService
    {
        public IEnumerable<UserMessage> All();

        public Task Add(UserMessage userMessage);

        public Task Delete(string id);

        public Task SetToRead(string id, bool isRead);

        public UserMessage GetById(string id);
    }
}
