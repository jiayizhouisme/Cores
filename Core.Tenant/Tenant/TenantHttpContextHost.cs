using Core.HttpTenant;
using Furion;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpTenant
{
    public class TenantHttpContextHost : ITenantGetSetor
    {
        public string Get()
        {
            if (App.HttpContext != null)
            {
                var head = App.HttpContext.Request.Host.ToString();
                return head;
            }
            return null;
        }
        public void Set(string tenant_name)
        {
        }
    }
}
