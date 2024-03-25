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
            try
            {
                Microsoft.AspNetCore.Identity.SignInResult signInResult;
                ApplicationUser applicationUser = _signInManager.UserManager.FindByNameAsync(username).Result;
                var role = _signInManager.UserManager.GetRolesAsync(applicationUser).Result;
                if (applicationUser == null)
                {
                    ViewBag.Error = "There is no such user";
                    return View("Index");
                }
                var isCompanyActive = _context.Companies!.Where(c => c.Id == applicationUser.CompanyId && c.StateId == 1).FirstOrDefault();
                if (isCompanyActive == null)
                {
                    if(!role.Contains("Admin"))
                    {
                        ViewBag.Error = "Your company is inactive. Please contact your administrator.";
                        return View("Index");
                    }
                }
                signInResult = _signInManager.PasswordSignInAsync(applicationUser, password, false, false).Result;
                if (signInResult.Succeeded)
                {
                    ViewBag.Succeed = true;
                    ViewBag.Error = null;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "An error occurred while logging in";
                    return View("Index");
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "ERROR !!!";
                return View("Index");
            }
        }

        public ActionResult Logout()
        {
            if (User.Identity!.IsAuthenticated)
            {
                _signInManager.SignOutAsync().Wait();
            }
            return RedirectToAction("Index", "Login");
        }

    }
}
