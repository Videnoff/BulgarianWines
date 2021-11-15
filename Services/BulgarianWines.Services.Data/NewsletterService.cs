namespace BulgarianWines.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class NewsletterService : INewsletterService
    {
        private readonly IDeletableEntityRepository<NewsletterSubscription> newsletterRepository;
        private readonly IDeletableEntityRepository<Contact> contactsRepository;
        private readonly DbContext subscriptionContext;
        private readonly ILogger logger;

        public NewsletterService(
            IDeletableEntityRepository<NewsletterSubscription> newsletterRepository,
            ILogger logger,
            IDeletableEntityRepository<Contact> contactsRepository,
            DbContext subscriptionContext)
        {
            this.newsletterRepository = newsletterRepository;
            this.logger = logger;
            this.contactsRepository = contactsRepository;
            this.subscriptionContext = subscriptionContext;
        }

        public async Task AddNewsletterSubscription(string email, string channel, string? firstname = null, string? lastname = null)
        {
            try
            {
                var contact = this.contactsRepository
                    .AllAsNoTracking()
                    .Include(c => c.NewsletterSubscriptions)
                    .SingleOrDefault(c => c.Email == email);

                // Create new contact if doesn't exist
                if (contact == null)
                {
                    contact = new Contact()
                    {
                        Email = email,
                        FirstName = firstname,
                        LastName = lastname,
                    };

                    await this.contactsRepository.AddAsync(contact);
                }

                await this.CreateAndSaveNewsletterSubscription(contact, channel).ConfigureAwait(false);

                this.logger.LogInformation("Added newsletter subscription successfully.");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Saving newsletter subscription failed");
                throw;
            }
        }

        public async Task<bool> RemoveNewsletterSubscription(string email, string channel)
        {
            try
            {
                var contact = this.contactsRepository
                    .AllAsNoTracking()
                    .Include(c => c.NewsletterSubscriptions)
                    .SingleOrDefault(c => c.Email == email);

                if (contact == null)
                {
                    return false;
                }

                var subscription = contact
                    .NewsletterSubscriptions
                    .SingleOrDefault(sub => sub.ChannelName == channel);

                if (subscription == null)
                {
                    return false;
                }

                this.newsletterRepository.Delete(subscription);

                await this.newsletterRepository.SaveChangesAsync().ConfigureAwait(false);

                this.logger.LogInformation("Removed newsletter subscription successfully.");

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Removing newsletter subscription failed");
                throw;
            }
        }

        private async Task CreateAndSaveNewsletterSubscription(Contact contact, string channel)
        {
            _ = contact.NewsletterSubscriptions ??= new List<NewsletterSubscription>();

            // Add newsletter subscription if doesn't exist
#pragma warning disable SA1305 // Field names should not use Hungarian notation
            if (contact.NewsletterSubscriptions.All(nSub => nSub.ChannelName != channel))
#pragma warning restore SA1305 // Field names should not use Hungarian notation
            {
                contact.NewsletterSubscriptions.Add(new NewsletterSubscription
                {
                    ChannelName = channel,
                });
            }

            await this.subscriptionContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
