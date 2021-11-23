namespace BulgarianWines.Web.ViewModels.Administration.UserMessages
{
    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class DeletedUserMessagesViewModel : IMapFrom<UserMessage>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string CreatedOn { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; }

        public string DeletedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserMessage, DeletedUserMessagesViewModel>()
                .ForMember(
                    x => x.DeletedOn,
                    opt => opt.MapFrom(x => x.DeletedOn.Value.ToString("f")));
        }
    }
}
