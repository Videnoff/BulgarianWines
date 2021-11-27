namespace BulgarianWines.Web.ViewModels.Administration.Components
{
    using System.Collections.Generic;

    using AutoMapper;
    using BulgarianWines.Web.ViewModels.Administration.UserMessages;

    public class NavbarViewModel
    {
        public NavbarViewModel()
        {
            this.MaxMessagesToDisplay = 5;
        }

        public IEnumerable<UserMessagesNavbarViewModel> UnreadUserMessages { get; set; }

        [IgnoreMap]
        public int MaxMessagesToDisplay { get; set; }
    }
}
