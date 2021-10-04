namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services.Data;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class CategoriesController : AdministrationController
    {
        private readonly IDeletableEntityRepository<Category> categoriesRepository;
        private readonly ICategoriesService categoriesService;

        public CategoriesController(
            IDeletableEntityRepository<Category> categoriesRepository,
            ICategoriesService categoriesService)
        {
            this.categoriesRepository = categoriesRepository;
            this.categoriesService = categoriesService;
        }

        // GET: Administration/Categories
        public async Task<IActionResult> Index()
        {
            return this.View(await this.categoriesRepository.AllWithDeleted().ToListAsync());
        }

        // GET: Administration/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.categoriesRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // GET: Administration/Categories/Create
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Administration/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Category category)
        {
            if (this.ModelState.IsValid)
            {
                await this.categoriesRepository.AddAsync(category);
                await this.categoriesRepository.SaveChangesAsync();

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(category);
        }

        // GET: Administration/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = this.categoriesRepository.All().FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return this.NotFound();
            }

            return View(category);
        }

        // POST: Administration/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Category category)
        {
            if (id != category.Id)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    this.categoriesRepository.Update(category);
                    await this.categoriesRepository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.CategoryExists(category.Id))
                    {
                        return this.NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return this.RedirectToAction(nameof(this.Index));
            }

            return this.View(category);
        }

        // GET: Administration/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var category = await this.categoriesRepository.All()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        // POST: Administration/Categories/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = this.categoriesRepository.All().FirstOrDefault(x => x.Id == id);

            this.categoriesRepository.Delete(category);
            await this.categoriesRepository.SaveChangesAsync();

            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Restore(int id)
        {
            var restoreResult = await this.categoriesService.RestoreAsync(id);

            if (restoreResult)
            {
                this.TempData["Alert"] = "Successfully restored category";
            }
            else
            {
                this.TempData["Error"] = "There was a problem restoring the category";
            }

            return this.RedirectToAction(nameof(this.Index));
        }

        private bool CategoryExists(int id)
        {
            return this.categoriesRepository.All().Any(e => e.Id == id);
        }
    }
}
