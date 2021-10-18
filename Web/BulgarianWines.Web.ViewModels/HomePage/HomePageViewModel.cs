namespace BulgarianWines.Web.ViewModels.HomePage
{
    using System.Linq;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Ganss.XSS;

    public class HomePageViewModel : IMapFrom<HomePageSlide>
    {
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }

        public string LinkUrl { get; set; }

        public string SanitizedDescription => new HtmlSanitizer().Sanitize(this.Description);
    }
}
