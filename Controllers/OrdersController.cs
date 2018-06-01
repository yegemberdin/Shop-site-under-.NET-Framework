using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projectnaz.Models;
using Microsoft.AspNet.Identity;

namespace Projectnaz.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private Database1 db = new Database1();

        
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        public string getTotalPrice(int orderID)
        {
            float totalPrice = 0;

            var orderDetails = db.OrderDetails.Where(i => i.OrderId == orderID).ToList();

            foreach (var od in orderDetails)
            {
                Product product = db.Products
                .Where(i => i.ProductId == od.ProductId)
                .FirstOrDefault();

                totalPrice += (od.Quantity * product.DiscountedPrice);
            }

            return totalPrice.ToString();
        }

        
        public ActionResult UpdateQuantity(int orderID, int orderDetailID, int amount)
        {
            string email = User.Identity.GetUserName();

            //Get the order detail
            OrderDetail orderDetail = db.OrderDetails
                .Where(j => j.OrderDetailId == orderDetailID)
                .FirstOrDefault();

            //Get the product information
            Product product = db.Products
                .Where(i => i.ProductId == orderDetail.ProductId)
                .FirstOrDefault();

            //Update the quantity and the total
            int previousQuantity = orderDetail.Quantity;
            orderDetail.Quantity = orderDetail.Quantity + amount;
            orderDetail.Quantity = ((orderDetail.Quantity <= 0) ? 1 : orderDetail.Quantity); //Dont let the quantity go below 1
            int newQuantity = orderDetail.Quantity;
            orderDetail.Quantity = ((orderDetail.Quantity > product.Stock) ? previousQuantity : newQuantity); //Dont let the quantity go above the stock of the product
            orderDetail.Total = product.DiscountedPrice * orderDetail.Quantity;
            db.Entry(orderDetail).State = EntityState.Modified;

            //Cart. Go through and update the cart total
            Order cart = db.Orders.FirstOrDefault(i => i.Email == email);
            db.Entry(cart).State = EntityState.Modified;

            if (previousQuantity != newQuantity)
            {
                if (amount < 0)
                {
                    cart.Total -= (product.DiscountedPrice * Math.Abs(amount));
                }
                else
                {
                    cart.Total += (product.DiscountedPrice * Math.Abs(amount));
                }
            }

            db.SaveChanges();

            return Redirect("/Orders/Details/" + orderID);
        }
        
        public ActionResult AddToCart(int id, int quantity)
        {
            string email = User.Identity.GetUserName();
            //find the product
            Product product = db.Products.First(i => i.ProductId == id);

            //create a temporary order detail instance based on the provided product id and quantity
            OrderDetail currentOrder = new OrderDetail();
            currentOrder.Quantity = quantity;
            currentOrder.Total = quantity * product.DiscountedPrice;
            currentOrder.ProductId = product.ProductId;

            if (email == null || email == "")
            {
                return Redirect("/Account/Register");
            }

            //if the user does not have a cart setup yet, create one
            //otherwise, update the user's cart
            Order cart = db.Orders.FirstOrDefault(i => i.Email == email);
            if (cart == null)
            {
                cart = new Order();
                cart.Email = email;
                cart.Total = product.DiscountedPrice * quantity;
                cart.OrderDate = DateTime.Now;

                //cart.OrderDetails = new List<OrderDetail>();
                //cart.OrderDetails.Add(currentOrder);

                db.Orders.Add(cart);
                db.OrderDetails.Add(currentOrder);
                db.SaveChanges();
            }
            else
            {
                //check if the cart has the same item
                //if it does, update the old order detail
                //if not, add the current order detail
                OrderDetail orderDetail = db.OrderDetails
                    .Where(i => i.OrderId == cart.OrderId)
                    .Where(j => j.ProductId == currentOrder.ProductId)
                    .FirstOrDefault();
                if (orderDetail == null)
                {
                    currentOrder.OrderId = cart.OrderId;
                    db.OrderDetails.Add(currentOrder);
                }
                else
                {
                    orderDetail.Quantity += currentOrder.Quantity;
                    orderDetail.Total += currentOrder.Total;
                    db.Entry(orderDetail).State = EntityState.Modified;
                    db.Entry(cart).State = EntityState.Modified;
                }
                cart.Total += currentOrder.Total;
                db.SaveChanges();
            }
            cart = db.Orders.FirstOrDefault(i => i.Email == email);

            // Go back to the products page
            return Redirect("/Orders/Details/" + cart.OrderId);
        }

     
        public ActionResult ViewCart()
        {
            string email = User.Identity.GetUserName();
            Order cart = db.Orders.FirstOrDefault(i => i.Email == email);
            if (cart == null)
            {
                cart = new Order();
                cart.Email = email;
                cart.Total = 0;
                cart.OrderDate = DateTime.Now;

                db.Orders.Add(cart);
                db.SaveChanges();
            }

            return Redirect("/Orders/Details/" + cart.OrderId);
        }

        
        public ActionResult Details(int? id)
        {
            ViewBag.getTotalPrice = new Func<int, string>(getTotalPrice);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            //lookup the list of orders
            //ViewBag.orders = db.OrderDetails.Where(i => i.OrderId == id).ToList();
            //lookup all the products involved in the order
            var query = (from o in db.OrderDetails
                         join p in db.Products on o.ProductId equals p.ProductId
                         where o.OrderId == id
                         select new OrderView
                         {
                             OrderDetail = o,
                             Product = p
                         });
            ViewBag.orders = query.ToList();
            return View(order);
        } 

       
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Purchased(int id)
        {

            //Delete Cart
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,Email,Total,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,Email,Total,OrderDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return Redirect("/Products"); //Goes back to the products page when the cart (Order) gets deleted 
        }

        protected override void Dispose(bool disposing)//lля ускорения очистки мусора, для того чтобы при финализации можно было определить сборщику мусора, что ресурс уже очищен вручную и нет нужды тратить время на то чтобы вновь очистить его. 

        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
