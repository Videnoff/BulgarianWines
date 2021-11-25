namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Web.ViewModels.Administration.UserMessages;
    using Microsoft.AspNetCore.Mvc;

    public class UserMessagesController : AdministrationController
    {
        private readonly IUserMessagesService userMessagesService;

        public UserMessagesController(IUserMessagesService userMessagesService)
        {
            this.userMessagesService = userMessagesService;
        }

        public IActionResult Index(string id)
        {
            var userMessages = this.userMessagesService
                .All<UserMessageViewModel>()
                .ToList();

            if (userMessages.Count == 0)
            {
                return this.View(new AllUserMessagesViewModel<UserMessageViewModel>());
            }

            var currentMessage = this.userMessagesService.GetById<UserMessageViewModel>(id);

            if (currentMessage == null)
            {
                currentMessage = userMessages.FirstOrDefault();
            }

            this.userMessagesService.SetToReadAsync(id, true);

            var viewModel = new AllUserMessagesViewModel<UserMessageViewModel>
            {
                UserMessageViewModelCollection = userMessages,
                UserMessageViewModel = currentMessage,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var result = await this.userMessagesService.DeleteAsync(id);

            if (result)
            {
                this.TempData["Alert"] = "Successfully deleted message.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the message.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Unread(string id)
        {
            var result = await this.userMessagesService.SetToReadAsync(id, false);

            if (result)
            {
                this.TempData["Alert"] = "Successfully marked message as unread";
            }
            else
            {
                this.TempData["Error"] = "There was a problem marking the message as unread.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Deleted(string id)
        {
            var userMessages = this.userMessagesService
                .AllDeleted<DeletedUserMessagesViewModel>()
                .ToList();

            if (userMessages.Count == 0)
            {
                return this.View(new AllUserMessagesViewModel<DeletedUserMessagesViewModel>());
            }

            var currentUserMessage = this.userMessagesService.GetById<DeletedUserMessagesViewModel>(id);

            if (currentUserMessage == null)
            {
                currentUserMessage = userMessages.FirstOrDefault();
            }

            this.userMessagesService.SetToReadAsync(id, true);

            var viewModel = new AllUserMessagesViewModel<DeletedUserMessagesViewModel>
            {
                UserMessageViewModelCollection = userMessages,
                UserMessageViewModel = currentUserMessage,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Restore(string id)
        {
            var result = await this.userMessagesService.RestoreAsync(id);

            if (result)
            {
                this.TempData["Alert"] = "Successfully restored message.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem restoring the message.";
            }

            return this.RedirectToAction(nameof(this.Deleted));
        }
    }
}
