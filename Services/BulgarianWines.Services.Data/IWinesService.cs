namespace BulgarianWines.Services.Data
{
    using System.Threading.Tasks;

    using BulgarianWines.Web.ViewModels.Wines;

    public interface IWinesService
    {
        Task CreateAsync(CreateWineInputModel input);
    }
}
