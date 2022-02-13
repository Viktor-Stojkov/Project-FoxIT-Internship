using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectMVC_FoxIT.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectMVC_FoxIT.Mappers;
using ProjectMVC_FoxIT.Models;
using WorkOrders.Shared;
using Postal.AspNetCore;

namespace ProjectMVC_FoxIT
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WorkOrdersContext>(options =>  // Changed AplicationDbContext to WorkOrdersContext to access route
                options.UseSqlServer(
                    Configuration.GetConnectionString("WorkOrdersConnection"))); // I created new Connection to the local machine

            services.AddDbContext<ApplicationDbContext>(options =>  // Changed AplicationDbContext to WorkOrdersContext to access route
                options.UseSqlServer(
                    Configuration.GetConnectionString("WorkOrdersConnection")));

            services.Configure<EmailSenderOptions>(Configuration.GetSection("EmailConfiguration"));  // Email
            services.AddPostal();
            services.AddTransient<IEmailSenderEnhance, EmailSender>();



            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();  // Changed AplicationDbContext

            services.AddAutoMapper(typeof(Startup));
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
             });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            /* https://stackoverflow.com/questions/40275195/how-to-set-up-automapper-in-asp-net-core */
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");
                endpoints.MapRazorPages();
            });
        }
    }
}
