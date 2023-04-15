using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.DataContexts;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using DataLayer.Data;
using WebApplication.Infrastructure;
using ServiceLayer.BookCatalogServices;
using ServiceLayer.BookCatalogServices.Concrete;
using ServiceLayer.CartServices;
using ServiceLayer.CartServices.Concrete;
using ServiceLayer.OrderServices;
using ServiceLayer.OrderServices.Concrete;
using ServiceLayer.Abstract;
using ServiceDbAccessLayer.Orders;
using ServiceDbAccessLayer.Orders.Concrete;

namespace WebApplication
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = configuration.GetConnectionString("BookApp_Identity");
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(connectionString: connectionString);
            });
            services.AddIdentity<AppUser, IdentityRole>(options=> 
            {
                // todo: review this settiongs
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddTransient<ICartLinesSessionSaver, CartLinesSessionSaver>();
            services.AddTransient<ICartService, SessionCartService>();
            services.AddTransient<IBookCatalogService, BookCatalogService>();
            services.AddTransient<IBookForCartService, BookForCartService>();
            services.AddTransient<IPlaceOrderService, PlaceOrderService>();
            services.AddTransient<IPlaceOrderDbAccess, PlaceOrderDbAccess>();
            services.AddTransient<ISignInContext, SignInContext>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                AppIdentityDbContext efDbContext = serviceProvider
                    .GetRequiredService<AppIdentityDbContext>();
 
                SeedData.RunSeed(
                    efDbContext: serviceProvider.GetRequiredService<AppIdentityDbContext>(),
                    roleManager: serviceProvider.GetRequiredService<RoleManager<IdentityRole>>(),
                    userManager: serviceProvider.GetRequiredService<UserManager<AppUser>>());
            }

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
