using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewData["Restaurant"] = new SelectList(_context.Set<Restaurant>(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,CompanyAdmin")]
        public ActionResult Create(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(category);
                    _context.SaveChanges();
                    return Ok("ok");
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
        }

        public ActionResult Edit(int? id)
        {
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;
            if (role.Contains("Admin") || role.Contains("CompanyAdmin"))
            {
                ViewBag.Authorized = true;
            }
            else
                ViewBag.Authorized = false;
            var category = _context.Categories!.Include(c => c.Restaurant).Where(c => c.Id == id).FirstOrDefault();
            if (category == null)
            {
                return NotFound(); 
            }
            

            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name", category.StateId);
            ViewData["Restaurant"] = new SelectList(_context.Set<Restaurant>(), "Id", "Name", category.Restaurant.Id);
            return View(category);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,CompanyAdmin")]
        public ActionResult Edit(Category category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(category);
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

        [Authorize(Roles = "Admin,CompanyAdmin")]
        [HttpPost]
        public ActionResult DeleteConfirmed(int Id)
        {
            try
            {
                var category = _context.Categories!.Where(c => c.Id == Id).FirstOrDefault();
                category!.StateId = 0;
                _context.Categories!.Update(category);
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

