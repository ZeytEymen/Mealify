using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mealify.Controllers
{
	public class CategoriesController : Controller
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;

        public CategoriesController(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ActionResult Index()
        {
            List<Category> categories = new List<Category>();
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin"))
            {
                categories = _context.Categories!.Include(c => c.State).Include(c => c.Restaurant).ToList();
                return View(categories);
            }
            if (role.Contains("CompanyAdmin"))
            {
                categories = _context.Categories!.Include(c => c.State).Include(c => c.Restaurant).ToList();
                return View(categories);
            }
           
            return View(categories);
        }
        public ActionResult GetCategories(int id)
        {
            var categories = _context.Categories!.Include(c => c.State).Include(c => c.Restaurant).Where(r => r.RestaurantId == id).ToList();
            return View(categories);
        }
    }
}

