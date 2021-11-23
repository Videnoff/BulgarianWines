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
            var userMessage = this.userMessagesService
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .ToList();

            if (userMessage.Count == 0)
            {
                return this.View(new AllUserMessagesViewModel<UserMessageViewModel>());
            }

            var currentMessage = this.userMessagesService.GetById(id);

            if (currentMessage == null)
            {
                currentMessage = userMessage.FirstOrDefault();
            }

            this.userMessagesService.SetToRead(id, true);

            var userMessageViewModelCollection = AutoMapperConfig.MapperInstance.Map<IEnumerable<UserMessageViewModel>>(userMessage);
            var currentUserMessageViewModel = AutoMapperConfig.MapperInstance.Map<UserMessageViewModel>(currentMessage);

            var viewModel = new AllUserMessagesViewModel<UserMessageViewModel>
            {
                UserMessageViewModelCollection = userMessageViewModelCollection,
                UserMessageViewModel = currentUserMessageViewModel,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.userMessagesService.Delete(id);

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Unread(string id)
        {
            await this.userMessagesService.SetToRead(id, false);

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Deleted(string id)
        {
            var userMessages = this.userMessagesService
                .AllWithDeleted()
                .Where(x => x.IsDeleted)
                .OrderByDescending(x => x.DeletedOn)
                .ToList();

            if (userMessages.Count == 0)
            {
                return this.View(new AllUserMessagesViewModel<DeletedUserMessagesViewModel>());
            }

            var currentUserMessage = this.userMessagesService.GetById(id);

            if (currentUserMessage == null)
            {
                currentUserMessage = userMessages.FirstOrDefault();
            }

            this.userMessagesService.SetToRead(id, true);

            var userMessageViewModelCollection = AutoMapperConfig.MapperInstance.Map<IEnumerable<DeletedUserMessagesViewModel>>(userMessages);
            var currentUserMessageViewModel =
                AutoMapperConfig.MapperInstance.Map<DeletedUserMessagesViewModel>(currentUserMessage);

            var viewModel = new AllUserMessagesViewModel<DeletedUserMessagesViewModel>
            {
                UserMessageViewModelCollection = userMessageViewModelCollection,
                UserMessageViewModel = currentUserMessageViewModel,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Restore(string id)
        {
            await this.userMessagesService.Restore(id);

            return this.RedirectToAction(nameof(this.Deleted));
        }
    }
}
