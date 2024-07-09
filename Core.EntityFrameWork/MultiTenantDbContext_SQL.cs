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
    public class MultiTenantDbContext_SQL : AppDbContext<MultiTenantDbContext_SQL, MultiTenantDbContextLocator>
    {
        public MultiTenantDbContext_SQL(DbContextOptions<MultiTenantDbContext_SQL> options) : base(options)
        {
            int i = 1;
            InsertOrUpdateIgnoreNullValues = true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = App.Configuration["ConnectionStrings:SqlConnection"];
            optionsBuilder.UseSqlServer(connStr, options =>
            {
                options.MigrationsAssembly(App.Configuration["ConnectionStrings:Migrations"]);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
