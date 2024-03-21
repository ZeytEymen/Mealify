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
            var companies = _context.Companies.Include(c => c.State).Include(c => c.ParentCompany).ToList();
            return View(companies);
        }

        public ActionResult Edit(int? id)
        {
            var company =  _context.Companies!.FindAsync(id).Result;
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

                    // Hata mesajlarını ViewBag üzerinden View'a aktarabiliriz
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
            return View();
        }

    }
}

