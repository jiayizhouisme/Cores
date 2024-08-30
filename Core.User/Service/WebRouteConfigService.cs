using Core.Services;
using Core.User.Entity;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User.Service
{
    public class WebRouteConfigService : BaseService<WebRouteConfig, MasterDbContextLocator>, IWebRouteConfigService, ITransient
    {
        public WebRouteConfigService(IRepository<WebRouteConfig, MasterDbContextLocator> _dal) {
            this._dal = _dal;
        }
        public async Task<WebRouteConfig> GetConfig(string keyPath)
        {
           return await GetQueryableNt(a => a.keyPath == keyPath).FirstOrDefaultAsync();
        }

        public async Task<ICollection<WebRouteConfig>> GetConfigs()
        {
            return await this.GetQueryableNt().ToListAsync();
        }
    }
}
