using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Projectnaz.Models
{
    public class Database1 : DbContext
    {
        

        public Database1() : base("name=DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<Projectnaz.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<Projectnaz.Models.Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Projectnaz.Models.OrderDetail> OrderDetails { get; set; }

    }
}
