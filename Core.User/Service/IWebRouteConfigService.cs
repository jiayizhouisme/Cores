using Core.Services;
using Core.User.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User.Service
{
    public interface IWebRouteConfigService : IBaseService<WebRouteConfig>
    {
        Task<ICollection<WebRouteConfig>> GetConfigs();
        Task<WebRouteConfig> GetConfig(string keyPath);

    }
}
