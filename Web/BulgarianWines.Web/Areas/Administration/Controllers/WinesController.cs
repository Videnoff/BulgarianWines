namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Azure.Storage.Blobs;
    using BulgarianWines.Data;
    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.Wines;
    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

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
        private readonly IDeletableEntityRepository<Wine> winesRepository;

        private readonly string fullDirectoryPath;
        private readonly ApplicationDbContext db;

        public WinesController(
            ICategoriesService categoriesService,
            IWinesService winesService,
            IVolumesService volumesService,
            IHarvestsService harvestsService,
            IVarietiesService varietiesService,
            IOriginsService originsService,
            BlobServiceClient blobServiceClient,
            IWebHostEnvironment webHostEnvironment,
            IDeletableEntityRepository<Wine> winesRepository,
            ApplicationDbContext db)
        {
            this.categoriesService = categoriesService;
            this.winesService = winesService;
            this.volumesService = volumesService;
            this.harvestsService = harvestsService;
            this.varietiesService = varietiesService;
            this.originsService = originsService;
            this.blobServiceClient = blobServiceClient;
            this.webHostEnvironment = webHostEnvironment;
            this.winesRepository = winesRepository;
            this.db = db;

            this.fullDirectoryPath = this.webHostEnvironment.WebRootPath + ProductsDirectoryPath;
        }

        // GET: Administration/Wines
        public IActionResult Index(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            const int itemsPerPage = 12;

            var wines = this.winesService.GetAll<ProductViewModel>(id, itemsPerPage);
            return this.View(wines);

        }

        // GET: Administration/Wines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wine = await this.db.Wines
                .Include(w => w.Category)
                .Include(w => w.Harvest)
                .Include(w => w.Origin)
                .Include(w => w.User)
                .Include(w => w.Variety)
                .Include(w => w.Volume)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wine == null)
            {
                return this.NotFound();
            }

            return this.View(wine);
        }

        // GET: Administration/Wines/Create
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

        // POST: Administration/Wines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Description,OriginId,ImageUrl,VarietyId,CategoryId,VolumeId,HarvestId,UserId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Wine wine)
        //{
        //    if (this.ModelState.IsValid)
        //    {
        //        this.db.Add(wine);
        //        await this.db.SaveChangesAsync();
        //        return this.RedirectToAction(nameof(this.Index));
        //    }

        //    this.ViewData["CategoryId"] = new SelectList(this.db.Categories, "Id", "Id", wine.CategoryId);
        //    this.ViewData["HarvestId"] = new SelectList(this.db.Harvests, "Id", "Id", wine.HarvestId);
        //    this.ViewData["OriginId"] = new SelectList(this.db.Origins, "Id", "Id", wine.OriginId);
        //    this.ViewData["UserId"] = new SelectList(this.db.Users, "Id", "Id", wine.UserId);
        //    this.ViewData["VarietyId"] = new SelectList(this.db.Varieties, "Id", "Id", wine.VarietyId);
        //    this.ViewData["VolumeId"] = new SelectList(this.db.Volumes, "Id", "Id", wine.VolumeId);

        //    return this.View(wine);
        //}

        [HttpPost]
        public async Task<IActionResult> Create(CreateWineInputModel input)
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

        // GET: Administration/Wines/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            //if (id == null)
            //{
            //    return this.NotFound();
            //}

            //var wine = await this.db.Wines.FindAsync(id);

            //if (wine == null)
            //{
            //    return this.NotFound();
            //}

            //this.ViewData["CategoryId"] = new SelectList(this.db.Categories, "Id", "Id", wine.CategoryId);
            //this.ViewData["HarvestId"] = new SelectList(this.db.Harvests, "Id", "Id", wine.HarvestId);
            //this.ViewData["OriginId"] = new SelectList(this.db.Origins, "Id", "Id", wine.OriginId);
            //this.ViewData["UserId"] = new SelectList(this.db.Users, "Id", "Id", wine.UserId);
            //this.ViewData["VarietyId"] = new SelectList(this.db.Varieties, "Id", "Id", wine.VarietyId);
            //this.ViewData["VolumeId"] = new SelectList(this.db.Volumes, "Id", "Id", wine.VolumeId);

            //var categories = this.categoriesService.GetAll();
            //var inputModel = this.winesService.GetById<EditProductViewModel>(id);

            //if (inputModel == null)
            //{
            //    this.TempData["Error"] = "Product not found.";
            //    return this.RedirectToAction(nameof(this.Index));
            //}

            //inputModel.Categories = categories;

            //return this.View(inputModel);

            var product = this.winesService.GetById<EditProductViewModel>(id);

            if (product == null)
            {
                this.TempData["Error"] = "Product not found!";
                return this.RedirectToAction(nameof(this.Index));
            }

            product.CategoriesItems = this.categoriesService.GetAllAsKeyValuePairs();
            product.VolumesItems = this.volumesService.GetAllAsKeyValuePairs();
            product.HarvestsItems = this.harvestsService.GetAllAsKeyValuePairs();
            product.VarietyItems = this.varietiesService.GetAllAsKeyValuePairs();
            product.OriginsItems = this.originsService.GetAllAsKeyValuePairs();

            return this.View(product);
        }

        // POST: Administration/Wines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditProductViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var result = await this.winesService.EditAsync<EditProductViewModel>(model, model.UploadedImages, this.fullDirectoryPath, this.webHostEnvironment.WebRootPath);

            if (result)
            {
                this.TempData["Alert"] = "Successfully edited product.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the product.";
            }

            return this.RedirectToAction(nameof(this.SingleWine), new {area = string.Empty, id});
        }

        // GET: Administration/Wines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wine = await this.db.Wines
                .Include(w => w.Category)
                .Include(w => w.Harvest)
                .Include(w => w.Origin)
                .Include(w => w.User)
                .Include(w => w.Variety)
                .Include(w => w.Volume)
                .Include(w => w.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wine == null)
            {
                return this.NotFound();
            }

            return this.View(wine);
        }

        // POST: Administration/Wines/Delete/5
        //[HttpPost]
        //[ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var wine = await this.db.Wines.FindAsync(id);
        //    this.db.Wines.Remove(wine);
        //    await this.db.SaveChangesAsync();
        //    return this.RedirectToAction(nameof(this.Index));
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleteResult = await this.winesService.DeleteAsync(id);

            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted product.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the product.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Restore(int id)
        {
            var restoreResult = await this.winesService.RestoreAsync(id);

            if (restoreResult)
            {
                this.TempData["Alert"] = "Successfully restored product";
            }
            else
            {
                this.TempData["Error"] = "There was a problem restoring the product";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Deleted()
        {
            var wines = this.winesService.GetAllDeleted<DeletedWineViewModel>();
            return this.View(wines);
        }

        public IActionResult SingleWine(int id)
        {
            var wine = this.winesService.GetById<SingleProductViewModel>(id);
            return this.View(wine);
        }

        public async Task<IActionResult> DeleteImage(string id)
        {
            var result = await this.winesService.DeleteImageAsync(id);

            if (result)
            {
                this.TempData["Alert"] = "Successfully deleted image!";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the image!";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        private bool WineExists(int id)
        {
            return this.db.Wines.Any(e => e.Id == id);
        }
    }
}
