namespace BulgarianWines.Web.ViewModels.Administration.UserMessages
{
    using System.Collections.Generic;

    public class AllUserMessagesViewModel<T>
    {
        public IEnumerable<T> UserMessageViewModelCollection { get; set; }

        public T UserMessageViewModel { get; set; }
    }
}
