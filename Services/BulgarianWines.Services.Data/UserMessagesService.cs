namespace BulgarianWines.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;

    public class UserMessagesService : IUserMessagesService
    {
        private readonly IDeletableEntityRepository<UserMessage> userMessagesRepository;

        public UserMessagesService(IDeletableEntityRepository<UserMessage> userMessagesRepository)
        {
            this.userMessagesRepository = userMessagesRepository;
        }

        public IEnumerable<UserMessage> All() => this.userMessagesRepository.AllWithDeleted();

        public async Task Add(UserMessage userMessage)
        {
            await this.userMessagesRepository.AddAsync(userMessage);
            await this.userMessagesRepository.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            var userMessage = this.GetById(id);
            if (userMessage == null)
            {
                return;
            }

            this.userMessagesRepository.Delete(userMessage);
            await this.userMessagesRepository.SaveChangesAsync();
        }

        public async Task SetToRead(string id, bool isRead)
        {
            var userMessage = this.GetById(id);
            if (userMessage == null)
            {
                return;
            }

            userMessage.IsRead = isRead;
            await this.userMessagesRepository.SaveChangesAsync();
        }

        public UserMessage GetById(string id) => this.userMessagesRepository.All().FirstOrDefault(x => x.Id == id);
    }
}
