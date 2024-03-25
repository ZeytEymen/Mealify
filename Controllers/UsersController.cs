using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mealify.Controllers
{
	public class UsersController : Controller
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;

        public UsersController(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ActionResult Index()
        {
            var users = _userManager.Users.Include(u => u.Company).Include(u => u.State).ToList();
            return View(users);
        }
        public ActionResult GetRestaurantUsers(int id)
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            var restaurants = _context.RestaurantUsers.Where(ru => ru.RestaurantId == id).ToList();
            foreach (var item in restaurants)
            {
                var user = _userManager.Users.Include(u => u.Company).Include(u => u.State).Where(u => u.Id == item.ApplicationUserId).FirstOrDefault();
                if (user != null)
                {
                    users.Add(user);
                }
            }
            return View(users);
        }

        public ActionResult Create()
        {
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin") || role.Contains("CompanyAdmin"))
            {
                ViewBag.Authorized = true;
            }
            else
                ViewBag.Authorized = false;

            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name");
            ViewData["Companies"] = new SelectList(_context.Set<Company>(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Create(ApplicationUser applicationUser, string password)
        {
            applicationUser.RegisterDate = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    var result= _userManager.CreateAsync(applicationUser, password).Result;
                    if (result.Succeeded)
                    {
                        return Ok("ok");
                    }
                    
                    //eturn RedirectToAction(nameof(Index));
                }
                else
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();
                    var errorMessage = string.Join(" ", errorMessages);

                    return Ok(errorMessage);
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            
            return View();
        }

    }
}

