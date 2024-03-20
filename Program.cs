using Mealify.Data;
using Mealify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mealify
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("ApplicationDatabase")));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            ApplicationContext? context = app.Services.CreateScope().ServiceProvider.GetService<ApplicationContext>();
            RoleManager<IdentityRole>? roleManager = app.Services.CreateScope().ServiceProvider.GetService<RoleManager<IdentityRole>>();
            UserManager<ApplicationUser>? userManager = app.Services.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>();
            DBInitializer dBInitializer = new DBInitializer(context, roleManager, userManager);


            app.Run();
        }
    }
}