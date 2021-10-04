namespace BulgarianWines.Web.ViewModels.Wines
{
    using System.Collections.Generic;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class EditProductViewModel : CreateWineInputModel, IMapFrom<Wine>
    {
        public int Id { get; set; }

        public IEnumerable<Image> Images { get; set; }
    }
}
