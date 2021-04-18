using System;
using System.Linq;
using System.Net;
using CardsBlazor.Areas.Identity.Data;
using CardsBlazor.Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CardsBlazor.ApiControllers
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    public class AuthController : ControllerBase
    {
        private const string APIKEYNAME = "ApiKey";
        private readonly ILogger<AuthController> _logger;
        private readonly CardsAppContext _context;

        public AuthController(ILogger<AuthController> logger, CardsAppContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        [Route("Login")]
        [MapToApiVersion("2.0")]
        public IActionResult Login([FromBody]AuthenticationViewModel authModel)
        {
            if (authModel == null) return BadRequest();
            if (string.IsNullOrWhiteSpace(authModel.UserToken)) return BadRequest();
            var model = _context.AppleAuthUsers.Where(x => x.DateArchived == null).FirstOrDefault(x => x.AppleAuthCode == authModel.UserToken);
            if (model == null)
            {
                _logger.LogWarning($"New AppleId SignIn - {authModel.FullName}");
                var authUser = new AppleAuthUser
                {
                    ApiKey = Guid.NewGuid(),
                    AppleAuthCode = authModel.UserToken,
                    EmailAddress = authModel.EmailAddress,
                    FullName = authModel.FullName,
                    DateArchived = null,
                    IsAdmin = false,
                    IsAllowedAccess = false
                };
                _context.AppleAuthUsers.Add(authUser);
                _context.SaveChanges();
                return StatusCode(402, authUser.ApiKey);
            }
            else
            {
                if(model.IsAllowedAccess) return Ok(model.ApiKey);
                return StatusCode((int) HttpStatusCode.PaymentRequired); //Use payment required as short hand for awaiting activation
            }

            return Unauthorized(); //Disallow login by default
        }

        [HttpGet]
        [Route("CheckKey")]
        [MapToApiVersion("2.0")]
        public IActionResult GetUpdatedPermissions()
        {
            try
            {
                if (!Request.Headers.TryGetValue(APIKEYNAME, out var apiKey)) return BadRequest();
                if (!Guid.TryParse(apiKey.ToString(), out var apiKeyConvert)) return BadRequest();
                var result = _context.AppleAuthUsers
                    .FirstOrDefault(x => x.ApiKey == apiKeyConvert);
                if (result == null) return NotFound();
                if (result.IsAllowedAccess) return Ok(apiKey);
                return StatusCode((int) HttpStatusCode.PaymentRequired);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while checking ApiKey");
                return StatusCode(500);
                throw;
            }
            
        }
    }

    public class AuthenticationViewModel
    {
        public string UserToken { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
    }

}