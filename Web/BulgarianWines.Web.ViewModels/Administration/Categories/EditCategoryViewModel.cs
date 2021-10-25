using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BulgarianWines.Web.Infrastructure.ValidationAttributes;

namespace BulgarianWines.Web.ViewModels.Administration.Categories
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Http;

    public class EditCategoryViewModel : CreateCategoryInputModel, IMapFrom<Category>
    {
        public int Id { get; set; }

        public IEnumerable<CategoryImage> CategoryImages { get; set; }
    }
}
