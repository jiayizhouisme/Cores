using Furion;
using Furion.LinqBuilder;

namespace Core.Config
{
    public class Configration
    {
        public static bool UseTenant { get; set; }
        public static string redisConnectionString { get; set; }
        public static string CachePrefix { get; set; }
        public static string ClusterId { get; set; }
        public static string DefaultSqlConnectionString { get;set; }
        public static string DefaultSqlMigrations { get; set; }
        public static bool UseCache { get; set; }
        public static string DbType { get { return App.Configuration["ConnectionStrings:DbType"]; }}
        public static TenantConfigTypes tenantConfigType { get; set; }

        public static void ConfigInit()
        {
            var _str = App.Configuration["ConnectionStrings:UseTenant"];

            UseTenant = !_str.IsNullOrEmpty() && _str.CompareTo("yes") == 0 ? true : false;
            string HostType = App.Configuration["ServerConfig:HostType"];

            _str = App.Configuration["ServerConfig:UseCache"];
            UseCache = !_str.IsNullOrEmpty() && _str.CompareTo("True") == 0 ? true : false;

            if (!string.IsNullOrEmpty(HostType))
            {
                tenantConfigType = (TenantConfigTypes)int.Parse(HostType);
            }
            else
            {
                tenantConfigType = TenantConfigTypes.ByHost;
            }

            redisConnectionString = App.Configuration["RedisConfig:ConnectionString"];
            CachePrefix = App.Configuration["ServerConfig:CachePrefix"];
            ClusterId = App.Configuration["ServerConfig:ClusterId"];
            DefaultSqlConnectionString = App.Configuration["ConnectionStrings:SqlConnection"];
            DefaultSqlMigrations = App.Configuration["ConnectionStrings:Migrations"];
        }
    }
}
