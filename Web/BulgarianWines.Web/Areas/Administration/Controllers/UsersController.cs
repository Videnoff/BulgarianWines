using System.Security.Claims;
using BulgarianWines.Data.Models.Claims;

namespace BulgarianWines.Web.Areas.Administration.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Services;
    using BulgarianWines.Web.ViewModels.Administration.Statistics;
    using BulgarianWines.Web.ViewModels.Administration.Users;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : AdministrationController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IUsersService usersService;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            IUsersService usersService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.usersRepository = usersRepository;
            this.usersService = usersService;
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return this.Json(true);
            }
            else
            {
                return this.Json($"Email {email} is already in use");
            }
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var applicationRole = new ApplicationRole
                {
                    Name = model.RoleName,
                };

                var result = await this.roleManager.CreateAsync(applicationRole);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("ListRoles", "Users");
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return this.View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = this.roleManager.Roles;
            return this.View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await this.roleManager.FindByIdAsync(id);

            if (role == null)
            {
                this.ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found!";
                return this.NotFound();
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            foreach (var user in this.userManager.Users)
            {
                if (await this.userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await this.roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                this.ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found!";
                return this.NotFound();
            }
            else
            {
                role.Name = model.RoleName;
                var result = await this.roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            this.ViewBag.roleId = roleId;

            var role = await this.roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                this.ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found!";
                return this.NotFound();
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in this.userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName,
                };

                if (await this.userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await this.roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                this.ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return this.NotFound();
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await this.userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !await this.userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await this.userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await this.userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await this.userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                    {
                        continue;
                    }
                    else
                    {
                        return this.RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }

            return this.RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = this.userManager.Users;
            return this.View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {id} cannot be found!";
                return this.NotFound();
            }

            var userClaims = await this.userManager.GetClaimsAsync(user);
            var userRoles = await this.userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Claims = userClaims.Select(x => x.Type + " : " + x.Value).ToList(),
                Roles = userRoles,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found!";
                return this.NotFound();
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.Username;

                var result = await this.userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return this.RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return this.View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var deleteResult = await this.usersService.DeleteUserAsync(id);

            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted user.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the user.";
            }

            return this.RedirectToAction(nameof(this.ListUsers));
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        [Authorize(Policy = "SuperAdminPolicy")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var deleteResult = await this.usersService.DeleteRoleAsync(id);

            if (deleteResult)
            {
                this.TempData["Alert"] = "Successfully deleted role.";
            }
            else
            {
                this.TempData["Error"] = "There was a problem deleting the role.";
            }

            return this.RedirectToAction(nameof(this.ListRoles));
        }

        public async Task<IActionResult> RestoreUser(string id)
        {
            var restoreResult = await this.usersService.RestoreUserAsync(id);

            if (restoreResult)
            {
                this.TempData["Alert"] = "Successfully restored user";
            }
            else
            {
                this.TempData["Error"] = "There was a problem restoring the user";
            }

            return this.RedirectToAction(nameof(this.ListUsers));
        }

        public IActionResult DeletedUsers()
        {
            var users = this.usersService.GetAllDeletedUsers<DeletedUsersViewModel>();
            return this.View(users);
        }

        public async Task<IActionResult> RestoreRole(string id)
        {
            var restoreResult = await this.usersService.RestoreRoleAsync(id);

            if (restoreResult)
            {
                this.TempData["Alert"] = "Successfully restored role";
            }
            else
            {
                this.TempData["Error"] = "There was a problem restoring the role";
            }

            return this.RedirectToAction(nameof(this.ListRoles));
        }

        public IActionResult DeletedRoles()
        {
            var roles = this.usersService.GetAllDeletedRoles<DeletedRolesViewModel>();
            return this.View(roles);
        }

        [HttpGet]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            this.ViewBag.userId = userId;

            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found!";
                return this.NotFound();
            }

            var model = new List<UsersRolesViewModel>();

            foreach (var role in this.roleManager.Roles)
            {
                var userRolesViewModel = new UsersRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                };

                if (await this.userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return this.View(model);
        }

        [HttpPost]
        [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> ManageUserRoles(List<UsersRolesViewModel> model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found!";
                return this.NotFound();
            }

            var roles = await this.userManager.GetRolesAsync(user);
            var result = await this.userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot remove user existing roles!");
                return this.View(model);
            }

            result = await this.userManager.AddToRolesAsync(user, model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot add selected roles to user!");
                return this.View(model);
            }

            return this.RedirectToAction("EditUser", new { Id = userId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return this.NotFound();
            }

            var existingUserClaims = await this.userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel()
            {
                UserId = userId,
            };

            foreach (var claim in ClaimsStore.AllClaims)
            {
                var userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                };

                if (existingUserClaims.Any(x => x.Type == claim.Type && x.Value == "true"))
                {
                    userClaim.IsSelected = true;
                }

                model.UserClaims.Add(userClaim);
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await this.userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                this.ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return this.NotFound();
            }

            // Get all the user existing claims and delete them
            var claims = await this.userManager.GetClaimsAsync(user);
            var result = await this.userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot remove user existing claims");
                return this.View(model);
            }

            // Add all the claims that are selected on the UI
            result = await this.userManager.AddClaimsAsync(user, model.UserClaims
                    .Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

            if (!result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, "Cannot add selected claims to user");
                return this.View(model);
            }

            return this.RedirectToAction("EditUser", new { Id = model.UserId });
        }
    }
}
