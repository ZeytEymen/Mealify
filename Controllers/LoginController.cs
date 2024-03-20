using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mealify.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationContext _context;

        public LoginController(SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ApplicationContext applicationContext)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = applicationContext;
        }
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "Users");
            }
            return View();
        }

        [HttpPost("Login")]
        public ActionResult Login(string username, string password)
        {
            Microsoft.AspNetCore.Identity.SignInResult signInResult;
            ApplicationUser applicationUser = _signInManager.UserManager.FindByNameAsync(username).Result;

            if (applicationUser == null)
            {
                return NotFound();
            }
            signInResult = _signInManager.PasswordSignInAsync(applicationUser, password, false, false).Result;
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return Ok();
        }

        public ActionResult Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                _signInManager.SignOutAsync().Wait();
            }
            return RedirectToAction("Index", "Login");
        }

    }
}
