using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        private IProductRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IProductRepository rep, IOrderProcessor proc)
        {
            repository = rep;
            orderProcessor = proc;

        }




        public ViewResult Index(Cart cart,string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }


        public RedirectToRouteResult AddToCart(int productid, string returnUrl,Cart cart)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productid);


            if (product != null)
            {
                cart.AddItem(product,1);
            }



            return RedirectToAction("Index", new {returnUrl});

        }


        public RedirectToRouteResult RemoveFromCart(Cart cart,int productid, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productid);

            if (product !=null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }


        public PartialViewResult Summary(Cart cart)
        {

            return PartialView(cart);
        }

        public ActionResult Checkout()
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }         
            else
            {
                return View(shippingDetails);
            }
        }
    }
}