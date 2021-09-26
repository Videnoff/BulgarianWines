﻿namespace BulgarianWines.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BulgarianWines.Web.ViewModels.Wines;
    using Microsoft.AspNetCore.Http;

    public interface IWinesService
    {
        Task CreateAsync<T>(T input, IEnumerable<IFormFile> images, string fullDirectoryPath, string webRootPath);

        IEnumerable<AllWinesViewModel> GetAll(int page, int itemsPerPage = 12);
    }
}
