using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth
{
    public class JwtUserContext : IHttpContextUser
    {
        private readonly IHttpContextAccessor _accessor;

        public JwtUserContext(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public string Name => _accessor.HttpContext.User.Identity.Name;

        public string ID => GetUserInfoFromToken("jti").FirstOrDefault();

        public string TenantId => GetTenantId();
        public string RealTenantId => GetRealTenantId();
        public string ClientIp => GetClientIp();
        public Permissions Permissions => GetPermissions();
        public string Agent => GetAgent();
        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string GetToken()
        {
            if (_accessor.HttpContext != null)
            {
                string _token = _accessor.HttpContext.Request.Headers["Authorization"];
                if (_token == null)
                {
                    _token = _accessor.HttpContext.Request.Query["access_token"];
                }
                if (_token != null)
                {
                    string token = _token.ToString().Replace("Bearer ", "");
                    if (token.IsNullOrEmpty())
                    {
                        token = _accessor.HttpContext.Request.Query["access_token"].ToString().Replace("Bearer ", "");
                    }
                    return token;
                }
            }
            return null;
        }

        public List<string> GetUserInfoFromToken(string ClaimType)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!string.IsNullOrEmpty(GetToken()))
            {
                JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(GetToken());

                return (from item in jwtToken.Claims
                        where item.Type == ClaimType
                        select item.Value).ToList();
            }
            else
            {
                return new List<string>() { };
            }
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return _accessor.HttpContext.User.Claims;
        }

        public List<string> GetClaimValueByType(string ClaimType)
        {
            return (from item in GetClaimsIdentity()
                    where item.Type == ClaimType
                    select item.Value).ToList();

        }

        private string GetTenantId()
        {
            if (_accessor.HttpContext != null)
            {
                return _accessor.HttpContext.Request.Host.Host + ":" + _accessor.HttpContext.Request.Host.Port;
            }
            return null;
        }

        private string GetRealTenantId()
        {
            if (_accessor.HttpContext != null)
            {
                return GetUserInfoFromToken("tenant-id").FirstOrDefault();
            }
            return null;
        }
        private string GetClientIp()
        {
            var realip = _accessor.HttpContext.GetRemoteIpAddressToIPv4();
            if (realip.IsNullOrEmpty())
            {
                realip = _accessor.HttpContext.GetRemoteIpAddressToIPv6();
            }
            return realip;
        }

        private Permissions GetPermissions()
        {
            var str = GetUserInfoFromToken("permissions").FirstOrDefault();
            if (!str.IsNullOrEmpty())
            {
                return (Permissions)int.Parse(str);
            }
            else
            {
                return Permissions.Unreconized;
            }

        }

        private string GetAgent()
        {
            return _accessor.HttpContext.Request.Headers["User-Agent"];
        }

    }
}
