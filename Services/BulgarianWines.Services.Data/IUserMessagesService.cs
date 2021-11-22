namespace BulgarianWines.Services.Data
{
    using System.Threading.Tasks;

    using BulgarianWines.Data.Models;

    public interface IUserMessagesService
    {
        public Task Add(UserMessage userMessage);

        public Task Delete(UserMessage userMessage);

        public Task SetToRead(string id, bool isRead);
    }
}
