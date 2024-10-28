﻿using Core.Auth;
using Core.Config;
using Core.HttpTenant;
using Furion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EntityFrameWork
{
    public class MasterDbContext_SQL<T> : DefaultDbContext<T> where T : DbContext
    {
        public MasterDbContext_SQL(DbContextOptions<T> options, ITenantGetSetor tenantGetSetor) : base(options, tenantGetSetor)
        {
            this.SetConnectString(Configration.DefaultSqlConnectionString);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = GetDatabaseConnectionString();
            optionsBuilder.UseSqlServer(connStr, options =>
            {
                options.MigrationsAssembly(Configration.DefaultSqlMigrations);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class MasterDbContext_SQL : DefaultDbContext<MasterDbContext_SQL>
    {
        public MasterDbContext_SQL(DbContextOptions<MasterDbContext_SQL> options, ITenantGetSetor tenantGetSetor) : base(options, tenantGetSetor)
        {
            this.SetConnectString(Configration.DefaultSqlConnectionString);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = GetDatabaseConnectionString();
            optionsBuilder.UseSqlServer(connStr, options =>
            {
                options.MigrationsAssembly(Configration.DefaultSqlMigrations);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
