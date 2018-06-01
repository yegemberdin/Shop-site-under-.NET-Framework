using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projectnaz.Models
{
    public class OrderView
    {
        public OrderDetail OrderDetail { get; set; }
        public Product Product { get; set; }
    }
}