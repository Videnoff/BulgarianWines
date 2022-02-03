namespace BulgarianWines.Web.ViewModels.Addresses
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class AddressViewModel : IMapFrom<Address>
    {
        public string Id { get; set; }

        public string Street { get; set; }

        public string Description { get; set; }

        public string CityName { get; set; }

        public string CityPostCode { get; set; }

        public string CityCountryName { get; set; }
    }
}
