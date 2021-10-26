namespace BulgarianWines.Web.ViewModels.HomePage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Ganss.XSS;
    using Microsoft.AspNetCore.Http;

    public class HomePageViewModel : IMapFrom<HomePageSlide>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string LinkUrl { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);

        [Display(Name = "Is Deleted?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Deleted on")]
        public DateTime? DeletedOn { get; set; }

        [Display(Name = "Created on")]
        public DateTime? CreatedOn { get; set; }

        [Display(Name = "Modified on")]
        public DateTime? ModifiedOn { get; set; }

        public IEnumerable<SlideImage> SlideImages { get; set; }
    }
}
