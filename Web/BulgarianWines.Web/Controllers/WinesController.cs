namespace BulgarianWines.Web.Controllers
{
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;

    public class WinesController : Controller
    {
        private readonly IWinesService winesService;

        public WinesController(IWinesService winesService)
        {
            this.winesService = winesService;
        }

        public IActionResult All(int id)
        {
            var viewModel = new WinesListViewModel
            {
                PageNumber = id,
                Wines = this.winesService.GetAll(id),
            };

            return this.View(viewModel);
        }
    }
}
