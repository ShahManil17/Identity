using ECommerceWithIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ECommerceWithIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ILogger<HomeController> logger, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
 
        public IActionResult Index()
        {
            if (Request.Cookies["ECommerceId"] != null)
            {
                return RedirectToAction("ListUsers", "User");
            }
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            Console.WriteLine();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            Response.Cookies.Delete("ECommerceId");
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
