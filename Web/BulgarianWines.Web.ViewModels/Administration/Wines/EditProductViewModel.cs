namespace BulgarianWines.Web.ViewModels.Administration.Wines
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.ViewModels.Wines;

    public class EditProductViewModel : CreateWineInputModel, IMapFrom<Wine>
    {
        public int Id { get; set; }

        public IEnumerable<Image> Images { get; set; }
    }
}
