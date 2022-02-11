namespace BulgarianWines.Web.Areas.Administration.Components
{
    using BulgarianWines.Data.Models.Enums;
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.Components;
    using BulgarianWines.Web.ViewModels.Administration.UserMessages;
    using Microsoft.AspNetCore.Mvc;

    public class NavbarViewComponent : ViewComponent
    {
        private readonly IUserMessagesService userMessagesService;
        private readonly ITimeService timeService;
        private readonly IOrdersService ordersService;

        public NavbarViewComponent(
            IUserMessagesService userMessagesService,
            ITimeService timeService,
            IOrdersService ordersService)
        {
            this.userMessagesService = userMessagesService;
            this.timeService = timeService;
            this.ordersService = ordersService;
        }

        public IViewComponentResult Invoke()
        {
            var unreadUserMessages = this.userMessagesService.GetUnreadMessages<UserMessagesNavbarViewModel>();
            var unProcessedOrdersCount = this.ordersService.GetOrdersCountByStatus(OrderStatus.Unprocessed) + this.ordersService.GetOrdersCountByStatus(OrderStatus.Processing);

            foreach (var message in unreadUserMessages)
            {
                message.TimePassedSinceSubmission = this.timeService.GetTimeSince(message.CreatedOn);
            }

            var viewModel = new NavbarViewModel
            {
                UnreadUserMessages = unreadUserMessages,
                UnprocessedOrdersCount = unProcessedOrdersCount,
            };

            return this.View(viewModel);
        }
    }
}
