using Core.Cache;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Furion.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.TenantJob
{
    /// <summary>
    /// 每天自动更新最新的库存
    /// </summary>
    public class TenantToCacheJob : IJob
    {
        private readonly ILogger<TenantToCacheJob> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICacheOperation _cache;
        public TenantToCacheJob(ILogger<TenantToCacheJob> _logger, IServiceProvider _serviceProvider, ICacheOperation _cache)
        {
            this._logger = _logger;
            this._serviceProvider = _serviceProvider;
            this._cache = _cache;
        }

        public async System.Threading.Tasks.Task ExecuteAsync(JobExecutingContext context, CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                await _cache.Del("Tenants");
                var te = Db.GetRepository<Tenant, MultiTenantDbContextLocator>(scope.ServiceProvider);
                var tenants = await te.AsQueryable().ToListAsync();
                foreach (var item in tenants)
                {
                    await _cache.PushToList("Tenants", item);
                }
                _logger.LogInformation("Tenant添加到缓存");
            }
               
        }
    }
}