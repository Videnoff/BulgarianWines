using BulgarianWines.Web.Areas.Administration.Security;

namespace BulgarianWines.Web
{
    using System;
    using System.IO;
    using System.Reflection;

    using Azure.Storage.Blobs;
    using BulgarianWines.Common;
    using BulgarianWines.Data;
    using BulgarianWines.Data.Common;
    using BulgarianWines.Data.Common.Repositories;
    using BulgarianWines.Data.Models;
    using BulgarianWines.Data.Repositories;
    using BulgarianWines.Data.Seeding;
    using BulgarianWines.Services;
    using BulgarianWines.Services.Data;
    using BulgarianWines.Services.Mapping;
    using BulgarianWines.Services.Messaging;
    using BulgarianWines.Web.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews(
                options =>
                    {
                        options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    }).AddRazorRuntimeCompilation().AddSessionStateTempDataProvider();
            services.AddRazorPages().AddSessionStateTempDataProvider();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(this.configuration);

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CreateRolePolicy", policy =>
                    policy
                        .RequireClaim("Create Role", "true"));

                options.AddPolicy("DeleteRolePolicy", policy =>
                    policy
                        .RequireClaim("Delete Role", "true"));

                options.AddPolicy("EditRolePolicy", policy =>
                    policy
                        .AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("SuperAdminPolicy", policy =>
                    policy
                        .RequireAssertion(context =>
                            context.User.IsInRole(GlobalConstants.SuperAdministratorRoleName)));
            });

            services.AddAuthentication()
#pragma warning disable SA1305 // Field names should not use Hungarian notation
                .AddGoogle(gOptions =>
#pragma warning restore SA1305 // Field names should not use Hungarian notation
                {
                    var googleAuthNSection = this.configuration.GetSection("Authentication:Google");

                    gOptions.ClientId = googleAuthNSection["ClientId"];
                    gOptions.ClientSecret = googleAuthNSection["ClientSecret"];
                });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromMinutes(60);
            });

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender>(x => new SendGridEmailSender(this.configuration["SendGrid:ApiKey"]));
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<ICategoriesService, CategoriesService>();
            services.AddTransient<IVolumesService, VolumesService>();
            services.AddTransient<IHarvestsService, HarvestsService>();
            services.AddTransient<IVarietiesService, VarietiesService>();
            services.AddTransient<IOriginsService, OriginsService>();
            services.AddTransient<IWinesService, WinesService>();
            services.AddTransient<IImagesService, ImagesService>();
            services.AddTransient<IHomePageSlidesService, HomePageSlidesService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IShortTextService, ShortTextService>();
            services.AddTransient<IAvailabilityService, AvailabilityService>();
            services.AddTransient<IFavoritesService, FavoritesService>();
            services.AddTransient<IUserMessagesService, UserMessagesService>();
            services.AddTransient<ITimeService, TimeService>();
            // services.AddTransient<INewsletterService, NewsletterService>();
            //File.WriteAllText("/home/abc" + Guid.NewGuid().ToString() + ".txt", this.configuration["BlobConnectionString"]);
            services.AddSingleton(x =>
                new BlobServiceClient(this.configuration["BlobConnectionString"]));
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                var adminCredentials = new AdminCredentials
                {
                    AdminUsername = this.configuration["AdminCredentials:AdminUsername"],
                    AdminPassword = this.configuration["AdminCredentials:AdminPassword"],
                    AdminEmail = this.configuration["AdminCredentials:AdminEmail"],
                };

                var superAdminCredentials = new SuperAdminCredentials
                {
                    SuperAdminUsername = this.configuration["SuperAdminCredentials:SuperAdminUsername"],
                    SuperAdminPassword = this.configuration["SuperAdminCredentials:SuperAdminPassword"],
                    SuperAdminEmail = this.configuration["SuperAdminCredentials:SuperAdminEmail"],
                };

                new ApplicationDbContextSeeder(adminCredentials, superAdminCredentials)
                    .SeedAsync(dbContext, serviceScope.ServiceProvider)
                    .GetAwaiter()
                    .GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Home/StatusCodePage", "?code={0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
