using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace Projectnaz.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public float DiscountedPrice { get; set; }
        public int Stock { get; set; }
        public int Purchases { get; set; }
        public string Picture { get; set; }
        public string Type { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Featured { get; set; }
    }
}