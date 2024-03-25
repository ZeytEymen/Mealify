using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewData["Category"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,CompanyAdmin")]
        public ActionResult Create(Food food)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(food);
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
            var food = _context.Foods!.Include(c => c.Category).Where(c => c.Id == id).FirstOrDefault();
            if (food == null)
            {
                return NotFound();
            }


            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name", food.StateId);
            ViewData["Category"] = new SelectList(_context.Set<Category>(), "Id", "Name", food.Category.Id);
            return View(food);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,CompanyAdmin")]
        public ActionResult Edit(Food food)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Foods.Update(food);
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
                var food = _context.Foods!.Where(c => c.Id == Id).FirstOrDefault();
                food!.StateId = 0;
                _context.Foods!.Update(food);
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

