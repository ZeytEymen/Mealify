using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Mealify.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;

        public RestaurantsController(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public ActionResult Index()
        {
            List<Restaurant> restaurants = new List<Restaurant>();
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin"))
            {
                restaurants = _context.Restaurants!.Include(r => r.State).Include(r => r.Company).ToList();
            }
            else if (role.Contains("CompanyAdmin"))
            {
                restaurants = _context.Restaurants!.Include(r => r.State).Include(r => r.Company).Where(r => r.Company!.ParentCompanyId == activeUser.CompanyId).ToList();
            }
            else if (role.Contains("RestaurantAdmin"))
            {
                List<RestaurantUser> restaurantUsers = _context.RestaurantUsers!.Include(ru => ru.Restaurant).Include(ru => ru.Restaurant!.Company).Where(ru => ru.ApplicationUserId == activeUser.Id).ToList();
                ViewData["RestaurantUsers"] = restaurantUsers;
                //restaurants = _context.Restaurants!.Include(r => r.State).Include(r => r.Company).Include(r => a)Where(r => r.Id == a.).ToList();
            }
            return View(restaurants);
        }

        public ActionResult GetRestaurants(int id)
        {
            var restaurant = _context.Restaurants!.Include(r => r.State).Include(r => r.Company).Where(r => r.CompanyId == id).ToList();
            return View(restaurant);
        }


        public ActionResult Create()
        {
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name");
            if (role.Contains("Admin"))
            {
                ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "Id", "Name");
                ViewBag.Authorized = true;
            }
            else if (role.Contains("CompanyAdmin"))
            {
               
              
            }
            else
                ViewBag.Authorized = false;

            return View();
        }




        // POST: RestaurantsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RestaurantsController/Edit/5
        public ActionResult Edit(int id)
        {
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin") || role.Contains("CompanyAdmin"))
            {
                ViewBag.Authorized = true;
            }
            else
                ViewBag.Authorized = false;
            var restaurant = _context.Restaurants!.FindAsync(id).Result;
            if (restaurant == null)
                return NotFound();

            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name", restaurant.StateId);
            return View(restaurant);
        }

        // POST: RestaurantsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RestaurantsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RestaurantsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
