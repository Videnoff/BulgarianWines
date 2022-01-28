namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Web.ViewModels.Addresses;

    public interface IAddressesService
    {
        public Task<bool> CreateAsync(AddressInputViewModel model);

        public IEnumerable<T> GetAll<T>(string userId);

        public Task<bool> DeleteAsync(string id);
    }
}
