using Furion;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Core.Auth
{
    public interface IHttpContextUser
    {
        string TenantId { get; }
        string Name { get; }
        string ID { get; }
        string RealTenantId { get; }
        string ExtraInfo { get; }
        string ClientIp { get; }
        Permissions Permissions { get; }
        string Agent { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
        List<string> GetClaimValueByType(string ClaimType);

        string GetToken();
        List<string> GetUserInfoFromToken(string ClaimType);
    }

    public enum Permissions
    {
        Administrator,
        Normal,
        Unreconized
    }
}