namespace BulgarianWines.Web.ViewModels.Administration.Dashboard
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class IndexViewModel : IMapTo<ApplicationUser>
    {
        public string ImageUrl { get; set; }
    }
}
