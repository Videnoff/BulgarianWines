namespace BulgarianWines.Web.Areas.Administration.Components
{
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.Components;
    using BulgarianWines.Web.ViewModels.Administration.UserMessages;
    using Microsoft.AspNetCore.Mvc;

    public class NavbarViewComponent : ViewComponent
    {
        private readonly IUserMessagesService userMessagesService;
        private readonly ITimeService timeService;

        public NavbarViewComponent(
            IUserMessagesService userMessagesService,
            ITimeService timeService)
        {
            this.userMessagesService = userMessagesService;
            this.timeService = timeService;
        }

        public IViewComponentResult Invoke()
        {
            var unreadUserMessages = this.userMessagesService.GetUnreadMessages<UserMessagesNavbarViewModel>();

            foreach (var message in unreadUserMessages)
            {
                message.TimePassedSinceSubmission = this.timeService.GetTimeSince(message.CreatedOn);
            }

            var viewModel = new NavbarViewModel
            {
                UnreadUserMessages = unreadUserMessages,
            };

            return this.View(viewModel);
        }
    }
}
