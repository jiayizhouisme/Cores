using Core.Config;
using Furion;
using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EntityFrameWork
{
    public class MultiTenantDbContext_MYSQL : AppDbContext<MultiTenantDbContext_MYSQL, MultiTenantDbContextLocator>
    {
        public MultiTenantDbContext_MYSQL(DbContextOptions<MultiTenantDbContext_MYSQL> options) : base(options)
        {
            InsertOrUpdateIgnoreNullValues = true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = Configration.DefaultSqlConnectionString;
            ServerVersion sv = ServerVersion.AutoDetect(connStr);
            optionsBuilder.UseMySql(connStr, sv, options =>
            {
                options.MigrationsAssembly(Configration.DefaultSqlMigrations);
            });
            base.OnConfiguring(optionsBuilder);
        }

    }
}
