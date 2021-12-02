﻿namespace BulgarianWines.Web.ViewModels.Wines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Ganss.XSS;

    public class ProductInfoViewModel : IMapFrom<Wine>, IHaveCustomMappings
    {
        private readonly HtmlSanitizer sanitizer;

        public ProductInfoViewModel()
        {
            this.sanitizer = new HtmlSanitizer();
            this.sanitizer.AllowedTags.Add("iframe");
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription => this.sanitizer.Sanitize(this.Description);

        public Category Category { get; set; }

        public IEnumerable<Image> Images { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        [IgnoreMap]
        public double AverageRating => (!this.Reviews.Any()) ? 0 : Math.Round(this.Reviews.Average(x => x.Rating), 2);

        [IgnoreMap]
        public double AverageRatingRounded => Math.Round(this.AverageRating * 2, MidpointRounding.AwayFromZero) / 2;

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Wine, ProductInfoViewModel>()
                .ForMember(
                    x => x.Reviews,
                    opt => opt.MapFrom(m => m.Reviews.OrderByDescending(x => x.CreatedOn)));
        }
    }
}
