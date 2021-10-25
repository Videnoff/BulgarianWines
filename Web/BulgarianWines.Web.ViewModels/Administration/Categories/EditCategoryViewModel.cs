namespace BulgarianWines.Web.ViewModels.Administration.Categories
{
    using System.Collections.Generic;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class EditCategoryViewModel : CreateCategoryInputModel, IMapFrom<Category>
    {
        public int Id { get; set; }

        public IEnumerable<CategoryImage> CategoryImages { get; set; }
    }
}
