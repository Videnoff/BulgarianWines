namespace BulgarianWines.Web.ViewModels.Wines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Ganss.XSS;

    public class SingleProductViewModel : IMapFrom<Wine>, IHaveCustomMappings
    {
        private readonly HtmlSanitizer sanitizer;

        public SingleProductViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add("iframe");
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string CategoryName { get; set; }

        public int CategoryId { get; set; }

        public string ImageUrl { get; set; }

        public string Origin { get; set; }

        public string OriginName { get; set; }

        public string Harvest { get; set; }

        public string HarvestYear { get; set; }

        public string Variety { get; set; }

        public string VarietyName { get; set; }

        public string Description { get; set; }

        public string Availability { get; set; }

        public string AvailabilityStatus { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public IEnumerable<WineReviewViewModel> Reviews { get; set; }

        public double AverageRating { get; set; }

        public IEnumerable<Image> WineImages { get; set; }

        public IEnumerable<Wine> Wines { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Wine, SingleProductViewModel>()
                .ForMember(x => x.ImageUrl, opt =>
                    opt.MapFrom(x =>
                        x.Images.FirstOrDefault().ImageUrl != null
                            ? x.Images.FirstOrDefault().ImageUrl
                            : "/images/wines/" + x.Images.FirstOrDefault().Id + "." + x.Images.FirstOrDefault()))
                .ForMember(
                    x => x.WineImages,
                    opt => opt.MapFrom(x => x.Images))
                .ForMember(
                    x => x.AverageRating,
                    opt => opt.MapFrom(m => (!m.Reviews.Any()) ? 0 : Math.Round(m.Reviews.Average(x => x.Rating), 2)));
        }
    }
}
