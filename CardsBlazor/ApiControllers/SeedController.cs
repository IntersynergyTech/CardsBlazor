using System;
using System.Threading.Tasks;
using CardsBlazor.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Core;

namespace CardsBlazor.ApiControllers
{
    [Route("api/Seed")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;
        private ILogger _logger;
        public SeedController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, ILogger logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SeedAdmin()
        {
            try
            {
                IdentityResult roleResult;
                //Adding Admin Role
                var roleCheck = await _roleManager.RoleExistsAsync("admin");
                if (!roleCheck)
                {

                    IdentityRole adminRole = new IdentityRole("admin");
                    //create the roles and seed them to the database
                    roleResult = await _roleManager.CreateAsync(adminRole);

                }

                var role2 = await _roleManager.RoleExistsAsync("user");
                if (!role2)
                {
                    IdentityRole adminRole = new IdentityRole("user");
                    //create the roles and seed them to the database
                    roleResult = await _roleManager.CreateAsync(adminRole);
                    _logger.Information("Seeded User Role");
                }
                var role3 = await _roleManager.RoleExistsAsync("moderator");
                if (!role3)
                {
                    IdentityRole adminRole = new IdentityRole("moderator");
                    //create the roles and seed them to the database
                    roleResult = await _roleManager.CreateAsync(adminRole);
                    _logger.Information("Seeded Moderator Role");
                }
                var userExists = await _userManager.FindByEmailAsync("admin@floul.dev");
                if (userExists == null)
                {
                    var appUser = new AppUser
                    {
                        Email = "admin@floul.dev",
                        EmailConfirmed = true,
                        UserName = "admin@floul.dev",
                        NormalizedEmail = "admin@floul.dev",
                    };
                    var res = _userManager.CreateAsync(appUser, "asd123asd").Result;
                    await _userManager.AddToRoleAsync(appUser, "admin");
                    _logger.Information("Seeded Admin Account");
                }
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}