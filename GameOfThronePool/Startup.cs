using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GameOfThronePool.Data;
using GameOfThronePool.Models;
using GameOfThronePool.Services;
using Microsoft.AspNetCore.DataProtection;
using System.IO;

namespace GameOfThronePool
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
            var environment = services.BuildServiceProvider().GetRequiredService<IHostingEnvironment>();

            services.AddMvc(
                options => options.EnableEndpointRouting = false);
            
            services.AddDbContext<DeadPoolDBContext>(options =>            
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            /*services.AddDefaultIdentity<IdentityUser>(config =>
            {
                config.SignIn.RequireConfirmedEmail = true;
            });*/
            services.AddIdentity<ApplicationUser, IdentityRole>()                
                .AddEntityFrameworkStores<DeadPoolDBContext>()
                .AddDefaultTokenProviders();
            
            //cookie configuration
            services.Configure<SecurityStampValidatorOptions>(options => 
                options.ValidationInterval = TimeSpan.FromDays(10));

            services.AddDataProtection()
                    .SetApplicationName($"my-app-{environment.EnvironmentName}")
                    .PersistKeysToFileSystem(new DirectoryInfo($@"{environment.ContentRootPath}\keys"));

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            services.AddSingleton<IConfiguration>(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {                
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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
