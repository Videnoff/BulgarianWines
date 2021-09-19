namespace BulgarianWines.Web.Controllers
{
    using System.Threading.Tasks;

    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Mvc;

    public class WinesController : Controller
    {
        private readonly ICategoriesService categoriesService;
        private readonly IWinesService winesService;
        private readonly IVolumesService volumesService;

        public WinesController(
            ICategoriesService categoriesService,
            IWinesService winesService,
            IVolumesService volumesService)
        {
            this.categoriesService = categoriesService;
            this.winesService = winesService;
            this.volumesService = volumesService;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateWineInputModel
            {
                CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs(),
                VolumesItems = this.volumesService.GetAllAsKeyValuePairs(),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateWineInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
                input.VolumesItems = this.volumesService.GetAllAsKeyValuePairs();
                return this.View(input);
            }

            await this.winesService.CreateAsync(input);

            return this.Redirect("/");
        }
    }
}
