using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth
{
    public class JwtUserContext_Real : JwtUserContext
    {
        public JwtUserContext_Real(IHttpContextAccessor accessor) : base(accessor)
        {
        }

        protected override string GetTenantId()
        {
            return base.GetRealTenantId();
        }
    }
}
