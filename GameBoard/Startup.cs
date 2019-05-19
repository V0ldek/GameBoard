using System;
using System.Globalization;
using System.Threading;
using GameBoard.Configuration;
using GameBoard.DataLayer.Entities;
using GameBoard.LogicLayer;
using GameBoard.LogicLayer.Configurations;
using GameBoard.LogicLayer.Notifications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameBoard
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureCookiePolicy(services);
            ConfigureAntiforgery(services);
            LoadConfiguration(services);
            ConfigureLogicLayer(services);
            ConfigureIdentity(services);
            ConfigureCulture();
            ConfigureMvc(services);
        }

        private static void ConfigureMvc(IServiceCollection services) =>
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        private static void ConfigureCulture() => CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");

        private void LoadConfiguration(IServiceCollection services)
        {
            services.Configure<MailNotificationsConfiguration>(
                Configuration.GetSection(nameof(MailNotificationsConfiguration)));
            services.Configure<HostConfiguration>(Configuration.GetSection(nameof(HostConfiguration)));
        }

        private static void ConfigureIdentity(IServiceCollection services)
        {
            services.AddDefaultIdentity<ApplicationUser>(
                    options =>
                    {
                        options.Password.RequiredLength = 8;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.User.RequireUniqueEmail = true;
                        options.SignIn.RequireConfirmedEmail = true;
                    })
                .AddDbContextStores();

            services.ConfigureApplicationCookie(
                options => { options.Cookie.Name = "GameBoard.Identity"; });
        }

        private void ConfigureLogicLayer(IServiceCollection services)
        {
            LogicLayer.Configuration.ConfigureDbContext(
                services,
                Configuration.GetConnectionString(
                    Environment.IsStaging() ? "GameboardStaging" :
                    Environment.IsProduction() ? "GameboardRelease" : "DefaultConnection"));

            LogicLayer.Configuration.ConfigureServices(services);
        }

        private static void ConfigureAntiforgery(IServiceCollection services) => services.AddAntiforgery(
            options =>
            {
                options.Cookie = new CookieBuilder
                {
                    Name = "GameBoard.Antiforgery",
                    SameSite = SameSiteMode.None,
                    HttpOnly = true,
                    IsEssential = true
                };
            });

        private static void ConfigureCookiePolicy(IServiceCollection services) =>
            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                    options.ConsentCookie = new CookieBuilder
                    {
                        Name = "GameBoard.Consent",
                        Expiration = TimeSpan.FromDays(365),
                        IsEssential = true
                    };
                });

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        "default",
                        "{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}