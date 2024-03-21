using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mealify.Data;
using Mealify.Models;

using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Http.Metadata;


namespace Mealify.Controllers
{
    [Authorize]
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

            List<Company> companies = new List<Company>();
            var activeUser = _userManager.GetUserAsync(User).Result;
            var role = _userManager.GetRolesAsync(activeUser).Result;

            if (role.Contains("Admin"))
            {
                companies = _context.Companies!.Include(c => c.State).Include(c => c.ParentCompany).ToList();
            }
            else if (role.Contains("CompanyAdmin"))
            {
                companies = _context.Companies!
                    .Include(c => c.State)
                    .Include(c => c.ParentCompany)
                    .Where(c => c.Id == activeUser.CompanyId || c.ParentCompanyId == activeUser.CompanyId).ToList();
            }
            else if (role.Contains("RestaurantAdmin"))
            {
                companies = _context.Companies!
                    .Include(c => c.State)
                    .Include(c => c.ParentCompany)
                    .Where(c => c.Id == activeUser.CompanyId).ToList();
            }
            return View(companies);
        }

        public ActionResult Edit(int? id)
        {
            var company = _context.Companies!.FindAsync(id).Result;
            if (company == null)
                return NotFound();

            ViewData["StateId"] = new SelectList(_context.Set<State>(), "Id", "Name", company.StateId);
            ViewData["ParentCompanyId"] = new SelectList(_context.Set<Company>(), "Id", "Name", company.ParentCompanyId);
            return View(company);
        }

        [HttpPost]
        public ActionResult Edit(Company company)
        {
            //var oldCompany = _context.Companies!.Where(r => r.Id == company.Id).FirstOrDefault();
            company.RegisterDate = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Update(company);
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
            }
            else
                ViewBag.Authorized = false;

            return View();
        }
        [Authorize(Roles = "Admin,CompanyAdmin")]
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var company = _context.Companies!.Where(c => c.Id == id).FirstOrDefault();
                company!.StateId = 0;
                _context.Companies!.Update(company);
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

