namespace BulgarianWines.Web.ViewModels.Administration.UserMessages
{
    using System.Collections.Generic;

    public class AllUserMessagesViewModel
    {
        public IEnumerable<UserMessageViewModel> UserMessageViewModelCollection { get; set; }

        public UserMessageViewModel UserMessageViewModel { get; set; }
    }
}
