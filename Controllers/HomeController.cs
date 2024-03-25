using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mealify.Data;
using Mealify.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Authorization;


namespace Mealify.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public HomeController(ApplicationContext context, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }


        public ActionResult Index()
        {
            //var company = _context.Companies!.Where(c => c.StateId == 1 && c.ParentCompany != null).ToList();
            var company = _context.Companies!.Where(c => c.StateId == 1).Include(c => c.Restaurants).Where(c => c.Restaurants!.Count() >= 1).ToList();
            ViewData["Companies"] = new SelectList(company, "Id", "Name");
            return View();
        }

        public JsonResult GetRestaurants(int CompanyId)
        {
            var restaurant = _context.Restaurants!.Where(r => r.StateId == 1 && r.CompanyId == CompanyId).ToList();
            return new JsonResult(restaurant);
        }

        [HttpPost, ActionName("GetFoodList")]
        public JsonResult GetFoodList(int RestaurantList)
        {
            var foods = _context.Foods!.Include(f => f.Category)
                .Where(f => f.Category!.RestaurantId == RestaurantList && f.StateId == 1)
                .ToList();

            return new JsonResult(foods.ToList());
        }

        public ActionResult About()
        {
            return View();
        }
    }
}