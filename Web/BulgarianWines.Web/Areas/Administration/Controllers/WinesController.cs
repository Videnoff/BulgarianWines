namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;

    public class WinesController : AdministrationController
    {
        private const string ProductsDirectoryPath = "\\images\\wines\\";

        private readonly ICategoriesService categoriesService;
        private readonly IWinesService winesService;
        private readonly IVolumesService volumesService;
        private readonly IHarvestsService harvestsService;
        private readonly IVarietiesService varietiesService;
        private readonly IOriginsService originsService;
        private readonly BlobServiceClient blobServiceClient;
        private readonly IWebHostEnvironment webHostEnvironment;

        private readonly string fullDirectoryPath;

        public WinesController(
            ICategoriesService categoriesService,
            IWinesService winesService,
            IVolumesService volumesService,
            IHarvestsService harvestsService,
            IVarietiesService varietiesService,
            IOriginsService originsService,
            BlobServiceClient blobServiceClient,
            IWebHostEnvironment webHostEnvironment)
        {
            this.categoriesService = categoriesService;
            this.winesService = winesService;
            this.volumesService = volumesService;
            this.harvestsService = harvestsService;
            this.varietiesService = varietiesService;
            this.originsService = originsService;
            this.blobServiceClient = blobServiceClient;
            this.webHostEnvironment = webHostEnvironment;

            this.fullDirectoryPath = this.webHostEnvironment.WebRootPath + ProductsDirectoryPath;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateWineInputModel
            {
                CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs(),
                VolumesItems = this.volumesService.GetAllAsKeyValuePairs(),
                HarvestsItems = this.harvestsService.GetAllAsKeyValuePairs(),
                VarietyItems = this.varietiesService.GetAllAsKeyValuePairs(),
                OriginsItems = this.originsService.GetAllAsKeyValuePairs(),
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
                input.HarvestsItems = this.harvestsService.GetAllAsKeyValuePairs();
                input.VarietyItems = this.varietiesService.GetAllAsKeyValuePairs();
                input.OriginsItems = this.originsService.GetAllAsKeyValuePairs();

                return this.View(input);
            }

            await this.winesService.CreateAsync<CreateWineInputModel>(input, input.UploadedImages, this.fullDirectoryPath, this.webHostEnvironment.WebRootPath);

            this.TempData["Alert"] = "Successfully created product.";

            return this.Redirect("/");
        }
    }
}
