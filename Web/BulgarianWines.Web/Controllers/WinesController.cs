using BulgarianWines.Web.ViewModels.Wines;
using Microsoft.AspNetCore.Mvc;

namespace BulgarianWines.Web.Controllers
{
    public class WinesController : Controller
    {
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateWineInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            return this.Redirect("/");
        }
    }
}