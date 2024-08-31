using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.HttpTenant
{
    public interface ITenantGetSetor
    {
        string Get();
        void Set(string tenant_name);
    }
}
