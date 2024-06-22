using ECommerceWithIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceWithIdentity.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult AccessDenied(string? returnUrl = null)
        {
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterView model)
        {
            if (ModelState.IsValid)
            {
                // Copy data from RegisterViewModel to IdentityUser
                var user = new IdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email
                };
                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);
                // If user is successfully created, sign-in the user using
                // SignInManager and redirect to index action of HomeController
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ListUsers", "User");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        
    }
}
