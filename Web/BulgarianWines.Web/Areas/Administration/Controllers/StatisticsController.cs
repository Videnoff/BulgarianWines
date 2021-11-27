﻿namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using BulgarianWines.Data.Models;
    using BulgarianWines.Web.ViewModels.Administration.Statistics;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("Administration/api/[controller]")]
    public class StatisticsController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public StatisticsController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet("UserRoles")]
        public IActionResult UserRoles()
        {
            var userRoles = new List<UserRolesViewModel>();

            var users = this.userManager.Users;
            var totalUsersCount = users.Count();

            var roles = this.roleManager.Roles;

            foreach (var role in roles)
            {
                var usersInRoleCount = users.Where(x => x.Roles.Any(y => y.RoleId == role.Id)).Count();

                userRoles.Add(new UserRolesViewModel
                {
                    RoleName = role.Name,
                    Percentage = Math.Round((double)usersInRoleCount / totalUsersCount * 100, 2),
                });
            }

            var normalUsersCount = users.Where(x => x.Roles.Count == 0).Count();
            userRoles.Add(new UserRolesViewModel
            {
                RoleName = "User",
                Percentage = Math.Round((double)normalUsersCount / totalUsersCount * 100, 2),
            });

            return this.Json(userRoles);
        }

        [HttpGet("RegisteredUsers")]
        public IActionResult RegisteredUsers()
        {
            var registeredUsers = new List<RegisteredUsersViewModel>();

            var userDates = this.userManager
                .Users
                .OrderBy(x => x.CreatedOn)
                .ToList()
                .GroupBy(x => x.CreatedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture));

            var totalusers = 0;

            foreach (var user in userDates)
            {
                totalusers += user.Count();
                registeredUsers.Add(new RegisteredUsersViewModel
                {
                    RegistrationDate = user.Key,
                    UsersCount = totalusers,
                });
            }

            return this.Json(registeredUsers);
        }
    }
}
