namespace BulgarianWines.Web.Areas.Identity.ViewModels
{
    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;
    using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

    public class RegisterViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }

        public string ImageUrl { get; set; }

        public string Code { get; set; }

        public string CallbackUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, RegisterViewModel>()
                .ForMember(
                    x => x.UserId,
                    opt => opt.MapFrom(m => m.Id));
            configuration.CreateMap<Pages.Account.RegisterModel, RegisterViewModel>()
                .ForMember(
                    x => x.FirstName,
                    opt => opt.MapFrom(m => m.Input.FirstName));
        }
    }
}
