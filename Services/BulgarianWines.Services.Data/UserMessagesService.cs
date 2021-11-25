namespace BulgarianWines.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class UserMessagesService : IUserMessagesService
    {
        private readonly IDeletableEntityRepository<UserMessage> userMessagesRepository;

        public UserMessagesService(IDeletableEntityRepository<UserMessage> userMessagesRepository)
        {
            this.userMessagesRepository = userMessagesRepository;
        }

        public IEnumerable<T> All<T>() => this.userMessagesRepository
            .AllAsNoTracking()
            .OrderByDescending(x => x.CreatedOn)
            .To<T>()
            .ToList();

        public IEnumerable<T> AllDeleted<T>() => this.userMessagesRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x => x.IsDeleted)
            .OrderByDescending(x => x.DeletedOn)
            .To<T>()
            .ToList();

        public async Task CreateAsync<T>(T model)
        {
            var userMessage = AutoMapperConfig.MapperInstance.Map<UserMessage>(model);

            await this.userMessagesRepository.AddAsync(userMessage);
            await this.userMessagesRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var userMessage = this.GetById(id);
            if (userMessage == null)
            {
                return false;
            }

            this.userMessagesRepository.Delete(userMessage);
            await this.userMessagesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RestoreAsync(string id)
        {
            var userMessage = this.GetById(id);

            if (userMessage == null)
            {
                return false;
            }

            this.userMessagesRepository.Undelete(userMessage);
            await this.userMessagesRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetToReadAsync(string id, bool isRead)
        {
            var userMessage = this.GetById(id);
            if (userMessage == null)
            {
                return false;
            }

            userMessage.IsRead = isRead;
            await this.userMessagesRepository.SaveChangesAsync();

            return true;
        }

        public T GetById<T>(string id) => this.userMessagesRepository
            .AllAsNoTrackingWithDeleted()
            .Where(x => x.Id == id)
            .To<T>()
            .FirstOrDefault();

        private UserMessage GetById(string id) =>
            this.userMessagesRepository
                .AllAsNoTrackingWithDeleted()
                .FirstOrDefault(x => x.Id == id);
    }
}
