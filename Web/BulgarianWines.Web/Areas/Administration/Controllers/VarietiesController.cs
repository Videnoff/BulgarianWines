namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Web.ViewModels.Administration.Varieties;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class VarietiesController : AdministrationController
    {
        private readonly IDeletableEntityRepository<Variety> varietiesRepository;
        private readonly IVarietiesService varietiesService;

        public VarietiesController(
            IDeletableEntityRepository<Variety> varietiesRepository,
            IVarietiesService varietiesService)
        {
            this.varietiesRepository = varietiesRepository;
            this.varietiesService = varietiesService;
        }

        // GET: Administration/Varieties
        public IActionResult Index()
        {
            var varieties = this.varietiesService.GetAll<VarietyVIewModel>();
            return this.View(varieties);
        }

        // GET: Administration/Varieties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var variety = await this.varietiesRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variety == null)
            {
                return this.NotFound();
            }

            return this.View(variety);
        }

        // GET: Administration/Varieties/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Varieties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVarietyInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.varietiesService.CreateAsync(input);

            this.TempData["Alert"] = "Successfully created variety.";

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Varieties/Edit/5
        public IActionResult Edit(int id)
        {
            var variety = this.varietiesService.GetById<EditVarietyViewModel>(id);

            if (variety == null)
            {
                this.TempData["Error"] = "Variety not found.";
                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(variety);
        }

        // POST: Administration/Varieties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditVarietyViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            var editResult = await this.varietiesService.EditAsync(model);
            if (editResult)
            {
                this.TempData["Alert"] = "Successfully edited variety.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem editing the variety.";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        // GET: Administration/Varieties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var variety = await this.varietiesRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (variety == null)
            {
                return this.NotFound();
            }

            return this.View(variety);
        }

        // POST: Administration/Varieties/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var variety = this.varietiesRepository.All().FirstOrDefault(x => x.Id == id);

            this.varietiesRepository.Delete(variety);
            await this.varietiesRepository.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Restore(int id)
        {
            var restoreResult = await this.varietiesService.RestoreAsync(id);

            if (restoreResult)
            {
                this.TempData["Alert"] = "Successfully restored variety";
            }
            else
            {
                this.TempData["Error"] = "There was a problem restoring the variety";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Deleted()
        {
            var varieties = this.varietiesService.GetAllDeleted<DeletedVarietyViewModel>();
            return this.View(varieties);
        }
    }
}
