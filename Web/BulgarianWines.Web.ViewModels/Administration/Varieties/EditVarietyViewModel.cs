namespace BulgarianWines.Web.ViewModels.Administration.Varieties
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class EditVarietyViewModel : CreateVarietyInputModel, IMapFrom<Variety>
    {
        public int Id { get; set; }
    }
}
