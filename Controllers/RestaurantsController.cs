using System.Data;
using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            }
            return View(restaurants);
        }

        public ActionResult GetRestaurants(int id)
        {
            var restaurant = _context.Restaurants!.Include(r => r.State).Include(r => r.Company).Where(r => r.CompanyId == id).ToList();
            return View(restaurant);
        }

        public ActionResult AddRestaurantUser(int restaurantId)
        {
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin") || role.Contains("CompanyAdmin"))
            {
                ViewBag.Authorized = true;
            }
            else
                ViewBag.Authorized = false;
            var users = _userManager.Users.Where(u => u.StateId == 1).ToList();
            return View(users);
        }


        public ActionResult Create()
        {
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin") || role.Contains("CompanyAdmin"))
            {
                ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name");
                ViewData["CompanyId"] = new SelectList(_context.Set<Company>(), "Id", "Name");
                ViewBag.Authorized = true;
            }
            else
                ViewBag.Authorized = false;
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,CompanyAdmin")]
        public ActionResult Create(Restaurant restaurant)
        {
            restaurant.RegisterDate = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(restaurant);
                    _context.SaveChanges();
                    return Ok("ok");
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
        }

        // GET: RestaurantsController/Edit/5
        public ActionResult Edit(int id)
        {
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin") || role.Contains("CompanyAdmin"))
            {
                ViewBag.Id = id;
                ViewBag.Authorized = true;
            }
            else
                ViewBag.Authorized = false;
            var restaurant = _context.Restaurants!.Include(r => r.Company).Include(r => r.State).Where(r => r.Id == id).FirstOrDefault();
            if (restaurant == null)
                return NotFound();

            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name", restaurant.StateId);
            return View(restaurant);
        }

        // POST: RestaurantsController/Edit/5
        [HttpPost]
        public ActionResult Edit(Restaurant restaurant)
        {
            restaurant.RegisterDate = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(restaurant);
                    _context.SaveChanges();
                    return Ok("ok");
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

        }

        // GET: RestaurantsController/Delete/5
        public ActionResult Delete(int id)
        {
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin") || role.Contains("CompanyAdmin"))
            {
                ViewBag.Authorized = true;
                ViewBag.Id = id;
            }
            else
                ViewBag.Authorized = false;

            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var restaurant = _context.Restaurants!.Where(r => r.Id == id).FirstOrDefault();
                restaurant!.StateId = 0;
                _context.Restaurants!.Update(restaurant);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
            return Ok("ok");
        }
    }
}
