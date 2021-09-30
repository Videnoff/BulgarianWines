﻿namespace BulgarianWines.Web.Controllers
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

        public IActionResult AllWines(int id = 1)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            const int itemsPerPage = 12;

            var viewModel = new WinesListViewModel
            {
                ItemsPerPage = itemsPerPage,
                PageNumber = id,
                WinesCount = this.winesService.GetCount(),
                Wines = this.winesService.GetAll<AllWinesViewModel>(id, itemsPerPage),
            };

            return this.View(viewModel);
        }

        public IActionResult SingleWine(int id)
        {
            var wine = this.winesService.GetById<SingleProductViewModel>(id);
            return this.View(wine);
        }
    }
}
