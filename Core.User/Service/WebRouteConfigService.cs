using Core.HttpTenant;
using Core.Services;
using Core.User.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User.Service
{
    public class WebRouteConfigService : BaseService<WebRouteConfig, MasterDbContextLocator>, IWebRouteConfigService
    {
        private readonly ITenantGetSetor tenantGetSetor;
        private readonly IMemoryCache _cache;
        public WebRouteConfigService(
            IRepository<WebRouteConfig, MasterDbContextLocator> _dal,
            ITenantGetSetor tenantGetSetor,
            IMemoryCache _cache) {
            this._dal = _dal;
            this.tenantGetSetor = tenantGetSetor;
            this._cache = _cache;
        }
        public async Task<WebRouteConfig> GetConfig(string keyPath)
        {
            var configs = await GetConfigs();
            if (keyPath == "/")
            {
                return configs.Where(a => a.keyPath == "/").FirstOrDefault();
            }
            foreach (var config in configs)
            {
                if (config.keyPath == "/")
                {
                    continue;
                }
                if (keyPath.StartsWith(config.keyPath))
                {
                    return config;
                }
            }
            return null;
            //return configs.Where(a => a.keyPath == keyPath).FirstOrDefault();
        }

        public async Task<ICollection<WebRouteConfig>> GetConfigs()
        {
            var tenant_name = tenantGetSetor.Get();
            var key = "WebRoutePaths:" + tenant_name;
            var paths = _cache.Get<List<WebRouteConfig>>(key);
            if (paths != null)
            {
                return paths;
            }
            var list = await this.GetQueryableNt().ToListAsync();
            _cache.Set(key, list);
            return list;
        }
    }
}
