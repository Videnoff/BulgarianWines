namespace BulgarianWines.Web.ViewModels.Administration.UserMessages
{
    using System;

    using AutoMapper;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Mapping;

    public class UserMessagesNavbarViewModel : IMapFrom<UserMessage>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Subject { get; set; }

        public string Email { get; set; }

        [IgnoreMap]
        public string TimePassedSinceSubmission { get; set; }
    }
}
