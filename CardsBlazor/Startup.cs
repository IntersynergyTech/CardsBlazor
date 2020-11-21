using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazor.Analytics;
using Blazor.Analytics.GoogleAnalytics;
using CardsBlazor.Areas.Identity;
using CardsBlazor.Areas.Identity.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CardsBlazor.Data;
using CardsBlazor.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog;
using Syncfusion.Blazor;

namespace CardsBlazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: "test",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
            });
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDbContext<CardsAppContext>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString("CardsAppContext"))
                    .ConfigureWarnings(c => c
                        .Log((RelationalEventId.CommandExecuting, LogLevel.Debug))
                        .Log((RelationalEventId.CommandExecuted, LogLevel.Debug))
                    ), ServiceLifetime.Transient);
            services.AddScoped<TokenProvider>();
            services.AddScoped<PlayerService>();
            services.AddScoped<GameService>();
            services.AddScoped<MatchService>();
            services.AddScoped<TokenProvider>();
            services.AddScoped<BoardService>();
            services.AddScoped<IdentityService>();
            services.AddScoped<CashGameService>();
            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.AddScoped<IAnalytics, GoogleAnalyticsStrategy>();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
            //services.AddDefaultIdentity<IdentityUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<IdentityContext>();
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });
            services.AddGoogleAnalytics("UA-167808686-1");
            services.AddSyncfusionBlazor();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjg1NTA2QDMxMzgyZTMyMmUzMEo4ZHJ6djJMTnpiOWlhcjB6ZXBBYU4yRlhUaG55ckowTUlUTlcvWUQ0akU9;Mjg1NTA3QDMxMzgyZTMyMmUzMEJEemticGpNeWNxczd5dTI0UlBWZitFZDl1RXkzcnBNQXBqME9LWmNoVUk9;Mjg1NTA4QDMxMzgyZTMyMmUzMG1adlE3dnhNTXhsRzdSNVg2RWJjWXlMMEFJRU5NTmxBQnRIZ2VrbURVcnM9;Mjg1NTA5QDMxMzgyZTMyMmUzMGZDaXlWdXY2bzgyY1Z5Ti8yeVhaRlA0ZWY4d3FaT2l5emRpb3hBOUw5QkU9;Mjg1NTEwQDMxMzgyZTMyMmUzMEhUL1RlR3VuQldQa2dySkJucUxjbzFBbDNuNjVTcFJxUk0xR0ZDRkVMQW89;Mjg1NTExQDMxMzgyZTMyMmUzMEV4TEJ5UmRhSy81UFdpMG9KOWxMNDRkQUJmYUcvNW8rRzBiZExCSXF6WFU9;Mjg1NTEyQDMxMzgyZTMyMmUzMG1icTFzNU9tYzF5eU1oRkEvaklyQmpUQXl5WG9kbG5FaHVQT3FWTTdVYVk9;Mjg1NTEzQDMxMzgyZTMyMmUzMG5DazFnbVlWWXU4MDc0aWxWelgwb3lMMUZ2UFJmT0o3VjBrbVZ4V29ZUFE9;Mjg1NTE0QDMxMzgyZTMyMmUzMFpqSndWQndhcHV2QVhaV05qUzZMaEhzMVMvbGZMY0xTd1Jlci9hL01GVk09;NT8mJyc2IWhia31hfWN9Z2doYmF8YGJ8ampqanNiYmlmamlmanMDHmg8OzI+NiFiYRM0PjI6P30wPD4=;Mjg1NTE1QDMxMzgyZTMyMmUzME9aNEZ0SGgyYXNOazloYUNjUGYwemozajZuSDdMV2VUcHVQcCtQd3F0Q1k9");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("test");
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //CreateUserRoles(provider);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
        }
    }
}
