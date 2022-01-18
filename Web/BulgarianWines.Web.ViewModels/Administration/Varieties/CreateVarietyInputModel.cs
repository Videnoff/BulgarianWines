namespace BulgarianWines.Web.ViewModels.Administration.Varieties
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class CreateVarietyInputModel : IMapTo<Variety>
    {
        public string Name { get; set; }

        [Display(Name = "Is Deleted?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted on")]
        public DateTime? DeletedOn { get; set; }

        [Display(Name = "Created on")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Modified on")]
        public DateTime? ModifiedOn { get; set; }
    }
}
