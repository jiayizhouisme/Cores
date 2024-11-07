using Core.Cache;
using Core.Config;
using Core.HttpTenant;
using Furion.DataEncryption.Extensions;
using Furion.LinqBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.MiddelWares
{
    public class CacheFilter : IAsyncResourceFilter
    {
        protected readonly ICacheOperation _cache;
        protected string suffixed;
        public const int _expireTime = 60;
        public readonly ITenantGetSetor tenantGetSetor;
        public CacheFilter(ICacheOperation _cache, ITenantGetSetor tenantGetSetor)
        {
            this._cache = _cache;
            this.tenantGetSetor = tenantGetSetor;
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
            if (Configration.UseCache == false)
            {
                await next();
            }

            var type = Configration.tenantConfigType;
            string host = context.HttpContext.Request.Host.ToString();
            string tenant_name = tenantGetSetor.Get();
            if (type != TenantConfigTypes.ByHost && !tenant_name.IsNullOrEmpty())
            {
                host = host + "/" + tenant_name;
            }
            string path = host + context.HttpContext.Request.Path +
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