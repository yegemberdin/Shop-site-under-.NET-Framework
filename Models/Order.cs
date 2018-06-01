using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projectnaz.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string Email { get; set; }
        public float Total { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}