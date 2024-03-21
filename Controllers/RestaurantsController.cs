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
                restaurants = _context.Restaurants!.Include(r => r.State).Include(r => r.Company).Where(r => r.CompanyId == activeUser.CompanyId).ToList();
            }
            else if (role.Contains("RestaurantAdmin"))
            {
                restaurants = _context.Restaurants!.Include(r => r.State).Include(r => r.Company).Where(r => r.CompanyId == activeUser.CompanyId).ToList();
            }
            return View(restaurants);
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
                List<SelectListItem> items = new List<SelectListItem>
                {
                    new SelectListItem { Value = activeUser.CompanyId.ToString(), Text = activeUser.Company!.Name.ToString() }
                };
                ViewData["CompanyId"] = new SelectList(items, "Id", "Name");
                ViewBag.Authorized = true;
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
            return View();
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
