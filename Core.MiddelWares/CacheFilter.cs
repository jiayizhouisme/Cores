using Core.Cache;
using Furion.DataEncryption.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.MiddelWares
{
    public class CacheFilter : IAsyncResourceFilter
    {
        protected readonly ICacheOperation _cache;
        protected string suffixed;
        public const int _expireTime = 3000;

        public CacheFilter(ICacheOperation _cache)
        {
            this._cache = _cache;
        }

        protected virtual async Task SetCache(ResourceExecutedContext context, string key)
        {
            await this._cache.Set(key, (context.Result as ObjectResult)?.Value, _expireTime);
        }

        protected virtual async Task<object> SetResult(ResourceExecutingContext context, string key)
        {
            var result = await this._cache.Get<object>(key);
            return result;
        }

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            string path = context.HttpContext.Request.Host + context.HttpContext.Request.Path +
                context.HttpContext.Request.QueryString.Value + suffixed;
            var key = path.ToMD5Encrypt(false, true);
            var result = await SetResult(context, key);
            if (result != null)
            {
                object obj = result;
                context.Result = new ObjectResult(obj);
            }
            else
            {
                ResourceExecutedContext execContext = await next();
                await SetCache(execContext, key);
            }
        }
    }
}