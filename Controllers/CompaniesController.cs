using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mealify.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;

        public CompaniesController(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var companies = _context.Companies.Include(c => c.State).Where(c => c.StateId == 1).ToList();
            return View(companies);
        }
    }
}

