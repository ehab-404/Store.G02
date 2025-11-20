using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

namespace Presentation.Attributes
{
    public class CacheAttribute(int durationInSeconds) : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CacheService;

            var cacheKey = GenerateCachekey(context.HttpContext.Request);

            var cachedValue = await cacheService.GetCachedValueAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedValue))
            {
                context.Result = new ContentResult()
                {
                    ContentType = "application/json",
                    Content = cachedValue
                };
                return;
            }
            var contextResult = await next();

            if (contextResult.Result is OkObjectResult okObjectResult)
            {
                await cacheService.SetCachedValueAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(durationInSeconds));
            }
        }


        private string GenerateCachekey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);

            foreach (var item in request.Query.OrderBy(q => q.Key))
            {
                key.Append($"|{item.Key}:{item.Value}");
            }

            return key.ToString();
        }
    }
}
