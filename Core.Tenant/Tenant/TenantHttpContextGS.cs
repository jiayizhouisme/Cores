using Furion;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpTenant
{
    public class TenantHttpContextGS : ITenantGetSetor,ISingleton
    {
        private readonly string TenantName_Key = "Tenant_Name";
        public string Get()
        {
            if (App.HttpContext != null)
            {
                var head = App.HttpContext.Request.Headers[TenantName_Key];
                return head;
            }
            return null;
        }
        public void Set(string tenant_name)
        {
            App.HttpContext.Request.Headers[TenantName_Key] = tenant_name;
        }
    }
}
