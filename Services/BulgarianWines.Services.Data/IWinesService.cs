using System.Collections.Generic;

namespace BulgarianWines.Services.Data
{
    using System.Threading.Tasks;

    using BulgarianWines.Web.ViewModels.Wines;

    public interface IWinesService
    {
        Task CreateAsync(CreateWineInputModel input);

        IEnumerable<AllWinesViewModel> GetAll(int page, int itemsPerPage = 12);
    }
}
