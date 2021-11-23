using System.Globalization;
using AutoMapper;

namespace BulgarianWines.Web.ViewModels.Administration.UserMessages
{
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class UserMessageViewModel : IMapFrom<UserMessage>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string CreatedOn { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserMessage, UserMessageViewModel>()
                .ForMember(
                    x => x.CreatedOn,
                    opt => opt.MapFrom(x => x.CreatedOn.ToString("f", CultureInfo.InvariantCulture)));
        }
    }
}
