namespace BulgarianWines.Services.Data
{
    using System.Threading.Tasks;

    public interface INewsletterService
    {
        public Task AddNewsletterSubscription(string email, string channel, string? firstname = null, string? lastname = null);

        public Task<bool> RemoveNewsletterSubscription(string email, string channel);
    }
}
