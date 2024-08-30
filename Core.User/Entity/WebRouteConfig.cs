using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User.Entity
{
    public class WebRouteConfig : IEntity<MasterDbContextLocator>
    {
        [Key]
        public int id { get; set; }
        public string keyPath { get; set; }
        public string routePath { get; set; }

    }
}
