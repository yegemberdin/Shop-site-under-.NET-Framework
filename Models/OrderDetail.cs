using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projectnaz.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
    }
}