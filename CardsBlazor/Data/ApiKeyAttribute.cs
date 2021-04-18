using System;
using System.Linq;
using System.Threading.Tasks;
using CardsBlazor.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CardsBlazor.Data
{
    [AttributeUsage(validOn: AttributeTargets.Class)]
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private readonly CardsAppContext _context;
        public ApiKeyAttribute(CardsAppContext context)
        {
            _context = context;
        }
        private const string APIKEYNAME = "ApiKey";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key was not provided"
                };
                return;
            }

            var result = _context.AppleAuthUsers.Where(x => x.DateArchived == null)
                .FirstOrDefault(x => x.ApiKey == extractedApiKey);
            if (result == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key is not valid"
                };
                return;
            }

            if (!result.IsAllowedAccess)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 402,
                    Content = "Awaiting ApiKey Activation"
                };
                return;
            }
 
            await next();
        }
        [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
        public class ApiKeyAdminAttribute : Attribute, IAsyncActionFilter
        {
            private readonly CardsAppContext _context;
            public ApiKeyAdminAttribute(CardsAppContext context)
            {
                _context = context;
            }
            private const string APIKEYNAME = "ApiKey";
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                if (!context.HttpContext.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "Api Key was not provided"
                    };
                    return;
                }

                var result = _context.AppleAuthUsers.Where(x => x.DateArchived == null && x.IsAdmin)
                    .FirstOrDefault(x => x.ApiKey == extractedApiKey);
                if (result == null)
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 401,
                        Content = "Api Key is not valid"
                    };
                    return;
                }
                if (!result.IsAllowedAccess)
                {
                    context.Result = new ContentResult()
                    {
                        StatusCode = 402,
                        Content = "Awaiting ApiKey Activation"
                    };
                    return;
                }
 
                await next();
            }
        }
    }
}