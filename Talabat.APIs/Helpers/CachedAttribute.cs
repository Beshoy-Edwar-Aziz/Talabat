using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLive;

        public CachedAttribute(int timeToLive)
        {
            _timeToLive = timeToLive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ResponseCacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var CacheKey = GenerateCacheKey(context.HttpContext.Request);
            var Response = await ResponseCacheService.GetResponseCacheAsync(CacheKey);
            if (Response is not null)
            {
                var result = new ContentResult()
                {
                    Content = Response,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = result;
                return;
            }
            var ExecuteContext = await next.Invoke();
            if(ExecuteContext.Result is OkObjectResult okObjectResult && okObjectResult.Value is not null)
            {
               await ResponseCacheService.ResponseCacheAsync(CacheKey, okObjectResult.Value,TimeSpan.FromSeconds(_timeToLive));
                return;
            }
        }

        private string GenerateCacheKey(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);
            foreach (var (key,value) in request.Query.OrderBy(O=>O.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
