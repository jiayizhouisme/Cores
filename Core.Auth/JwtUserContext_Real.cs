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


        public override string Name => GetUserInfoFromToken("name").FirstOrDefault();
        protected override string GetTenantId()
        {
            return base.GetRealTenantId();
        }
    }
}
