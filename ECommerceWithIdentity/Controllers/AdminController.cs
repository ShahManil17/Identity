using ECommerceWithIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ECommerceWithIdentity.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleView role)
        {
            if (ModelState.IsValid)
            {
                // Check if the role already exists
                bool roleExists = await _roleManager.RoleExistsAsync(role?.RoleName);
                if (roleExists)
                {
                    ModelState.AddModelError("", "Role Already Exists");
                }
                else
                {
                    // Create the role
                    // We just need to specify a unique role name to create a new role
                    IdentityRole identityRole = new IdentityRole
                    {
                        Name = role?.RoleName
                    };
                    // Saves the role in the underlying AspNetRoles table
                    IdentityResult result = await _roleManager.CreateAsync(identityRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return RedirectToAction("ListRoles", "Admin");
            }
            return View("CreateRole");
        }

        public async Task<IActionResult> ListRoles()
        {
            List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            //string returnUrl = Url.Content("~/");
            //returnUrl = returnUrl;
            return View(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(IFormCollection data)
        {
            Console.WriteLine(data["id"]);
            var role = await _roleManager.FindByIdAsync(data["id"]);
            if (role == null)
            {
                // Role not found, handle accordingly
                ViewBag.ErrorMessage = $"Role with Id = {data["id"]} cannot be found";
                return View("NotFound");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                // Role deletion successful
                return RedirectToAction("ListRoles"); // Redirect to the roles list page
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            // If we reach here, something went wrong, return to the view
            return View("ListRoles", await _roleManager.Roles.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(string roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                // Handle the scenario when the role is not found
                return View("NotFound");
            }
            //Populate the EditRoleViewModel from the data retrived from the database
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
                // You can add other properties here if needed
            };
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel data)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(data.Id);
                if (role == null)
                {
                    // Handle the scenario when the role is not found
                    ViewBag.ErrorMessage = $"Role with Id = {data.Id} cannot be found";
                    return View("NotFound");
                }
                else
                {
                    role.Name = data.RoleName;
                    // Update other properties if needed
                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles"); // Redirect to the roles list
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(data);
                }
            }
            return View(data);
        }
    }
}
