using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EscrowCore.Data;
using EscrowCore.Models;
using EscrowCore.Services;
using Ipfs.CoreApi;
using System.Net.Http;
//using Microsoft.AspNetCore.Http;
//using React.AspNet;


namespace EscrowCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //adding db and identity services
            services.AddDbContext<ApplicationDbContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            //Adding React services
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddReact();
            

            services.AddMvc();
            
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseReact(config =>
            //{
            //    // If you want to use server-side rendering of React components,
            //    // add all the necessary JavaScript files here. This includes
            //    // your components as well as all of their dependencies.
            //    // See http://reactjs.net/ for more information. Example:
            //    //config
            //    //    .AddScript("~/js/test.jsx");
            //    //    .AddScript("~/Scripts/Second.jsx");

            //    // If you use an external build too (for example, Babel, Webpack,
            //    // Browserify or Gulp), you can improve performance by disabling
            //    // ReactJS.NET's version of Babel and loading the pre-transpiled
            //    // scripts. Example:
            //    //config
            //    //    .SetLoadBabel(false)
            //    //    .AddScriptWithoutTransform("~/Scripts/bundle.server.js");
            //});

            app.UseStaticFiles();
            
            app.UseAuthentication();

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
