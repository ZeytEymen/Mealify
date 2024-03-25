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
            var categories = _context.Categories!.Include(c => c.State).Include(c => c.Restaurant).ToList();
            return View(categories);
        }
        public ActionResult GetCategories(int id)
        {
            var categories = _context.Categories!.Include(c => c.State).Include(c => c.Restaurant).Where(r => r.RestaurantId == id).ToList();
            return View(categories);
        }
    }
}

