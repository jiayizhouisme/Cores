﻿using Core.Auth;
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
    public class MasterDbContext_MYSQL<T> : DefaultDbContext<T> where T : DbContext
    {
        public MasterDbContext_MYSQL(DbContextOptions<T> options,ITenantGetSetor tenantGetSetor) : base(options, tenantGetSetor)
        {
            this.SetConnectString(App.Configuration["ConnectionStrings:SqlConnection"]);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = GetDatabaseConnectionString();
            ServerVersion sv = ServerVersion.AutoDetect(connStr);
            optionsBuilder.UseMySql(connStr, sv, options =>
            {
                options.MigrationsAssembly(App.Configuration["ConnectionStrings:Migrations"]);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }

    public class MasterDbContext_MYSQL : DefaultDbContext<MasterDbContext_MYSQL>
    {
        public MasterDbContext_MYSQL(DbContextOptions<MasterDbContext_MYSQL> options, ITenantGetSetor tenantGetSetors) : base(options, tenantGetSetors)
        {
            this.SetConnectString(App.Configuration["ConnectionStrings:SqlConnection"]);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = GetDatabaseConnectionString();
            ServerVersion sv = ServerVersion.AutoDetect(connStr);
            optionsBuilder.UseMySql(connStr, sv, options =>
            {
                options.MigrationsAssembly(App.Configuration["ConnectionStrings:Migrations"]);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
