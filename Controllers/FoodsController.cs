using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Mealify.Controllers
{
	public class FoodsController : Controller
	{
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;

        public FoodsController(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ActionResult Index()
        {
            var food = _context.Foods!.Include(f => f.State).Include(f => f.Category).ToList();
            return View(food);
        }
    }
}

