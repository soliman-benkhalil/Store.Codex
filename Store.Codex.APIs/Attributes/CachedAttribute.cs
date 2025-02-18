using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Store.Codex.Core.Services.Contract;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text;

namespace Store.Codex.APIs.Attributes
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    // by make the class inherits from the class attribute .. it got the properties of the attribute generally 
    // make the constructor to take a parameter .. i force any one use the attribute to send a parameter .. here represents the time 
    // if i want to make a logic on the attribute i have to implement the interface IAsyncActionFilter
    {
        private readonly int _expireTime;

        public CachedAttribute(int expireTime)
        {
            _expireTime = expireTime;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>(); // ask clr to make a object from IcachedService and speak with the in-memory database

            var cacheKey = GenerateCachedKeyFromRequest(context.HttpContext.Request);

            var cacheResponse = await cacheService.GetCacheKeyAsync(cacheKey);

            if(! string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };

                context.Result = contentResult;
                return;
            }
            var executedContext = await next();

            if (executedContext.Result is OkObjectResult response)
            {
                await cacheService.SetCacheKeyAsync(cacheKey,response.Value,TimeSpan.FromSeconds(_expireTime));   
            }

        }

        private string GenerateCachedKeyFromRequest(HttpRequest request) // key represents the shape of the data because all the data is not the same format
        {
            var cacheKey = new StringBuilder();
            cacheKey.Append($"{request.Path}");

            foreach(var (key,value) in request.Query.OrderBy(X=>X.Key)) // Query is a list that has all query prams in this request
            {
                cacheKey.Append($"|{key}-{value}");
            }
            return cacheKey.ToString();
        }

    }
}
