using Mealify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mealify.Data
{
    public class DBInitializer
    {
        public DBInitializer(ApplicationContext? context, RoleManager<IdentityRole>? roleManager, UserManager<ApplicationUser>? userManager)
        {
            State state;
            IdentityRole identityRole;
            ApplicationUser applicationUser;
            Company? company = null;

            if (context != null)
            {
                context.Database.Migrate();
                if (context.States!.Count() == 0)
                {
                    state = new State();
                    state.Id = 0;
                    state.Name = "Deleted";
                    context.States!.Add(state);
                    state = new State();
                    state.Id = 1;
                    state.Name = "Active";
                    context.States.Add(state);
                    state = new State();
                    state.Id = 2;
                    state.Name = "Passive";
                    context.States.Add(state);
                }
                if (context.Companies!.Count() == 0)
                {
                    company = new Company();
                    company.Name = "TAB Gıda Sanayi ve Ticaret A.Ş.";
                    company.WebAddress = "https://www.tabgida.com.tr";
                    company.Address = "Dikilitaş Mahallesi Emirhan Caddesi No:109 Beşiktaş / İstanbul";
                    company.EMail = "info@tabgida.com.tr";
                    company.Phone = "(0212) 310 66 00";
                    company.PostalCode = "34000";
                    company.RegisterDate = DateTime.Today;
                    company.StateId = 1;
                    company.TaxNumber = "8150037902";
                    context.Companies!.Add(company);
                }
                context.SaveChangesAsync().Wait();
                if (roleManager != null)
                {
                    if (roleManager.Roles.Count() == 0)
                    {
                        identityRole = new IdentityRole("Admin");
                        roleManager.CreateAsync(identityRole).Wait();
                        identityRole = new IdentityRole("CompanyAdmin");
                        roleManager.CreateAsync(identityRole).Wait();
                        identityRole = new IdentityRole("RestaurantAdmin");
                        roleManager.CreateAsync(identityRole).Wait();
                    }
                }
                if (userManager != null)
                {
                    if (userManager.Users.Count() == 0)
                    {
                        if (company != null)
                        {
                            applicationUser = new ApplicationUser();
                            applicationUser.UserName = "systemAdmin";
                            applicationUser.CompanyId = company.Id;
                            applicationUser.Name = "SystemAdministrator";
                            applicationUser.Email = "info@zeyteymen.com";
                            applicationUser.PhoneNumber = "+90 252 369 5454";
                            applicationUser.RegisterDate = DateTime.Today;
                            applicationUser.StateId = 1;
                            userManager.CreateAsync(applicationUser, "Admin123!").Wait();
                            userManager.AddToRoleAsync(applicationUser, "Admin").Wait();

                            applicationUser = new ApplicationUser();
                            applicationUser.UserName = "tabGidaAdmin";
                            applicationUser.CompanyId = company.Id;
                            applicationUser.Name = "Tab Gıda Admin";
                            applicationUser.Email = "info@tabgida_admin.com";
                            applicationUser.PhoneNumber = "+90 252 369 5454";
                            applicationUser.RegisterDate = DateTime.Today;
                            applicationUser.StateId = 1;
                            userManager.CreateAsync(applicationUser, "Admin123!").Wait();
                            userManager.AddToRoleAsync(applicationUser, "CompanyAdmin").Wait();

                        }
                    }
                }
            }
        }
    }
}
