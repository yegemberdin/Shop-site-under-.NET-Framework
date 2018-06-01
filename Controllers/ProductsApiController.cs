using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http; 
using Projectnaz.Models;

namespace Projectnaz.Controllers
{
    public class ProductsApiController : ApiController
    {

        public IEnumerable<Product> GetAllProducts()
        {
            Database1 db = new Database1();
            IEnumerable<Product> products = db.Products.ToList();
            return products;
        }
    }
}
