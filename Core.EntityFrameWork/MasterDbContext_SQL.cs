using Core.Auth;
using Furion;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EntityFrameWork
{
    public class MasterDbContext_SQL : DefaultDbContext<MasterDbContext_SQL>
    {
        public MasterDbContext_SQL(DbContextOptions<MasterDbContext_SQL> options, IHttpContextUser user) : base(options, user)
        {
            this.SetConnectString(App.Configuration["ConnectionStrings:SqlConnection"]);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = GetDatabaseConnectionString();
            optionsBuilder.UseSqlServer(connStr, options =>
            {
                options.MigrationsAssembly(App.Configuration["ConnectionStrings:Migrations"]);
            });
            base.OnConfiguring(optionsBuilder);
        }
    }
}
