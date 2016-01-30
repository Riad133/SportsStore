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

        public CartController(IProductRepository rep)
        {
            repository = rep;
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


        
        
    }
}