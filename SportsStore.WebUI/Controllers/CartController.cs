using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

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



        RedirectToRouteResult AddToCart(int productid, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productid);


            if (product != null)
            {
                GetCart().AddItem(product,1);
            }



            return RedirectToAction("Index", new {returnUrl});

        }


        public RedirectToRouteResult RemoveFromCart(int productid, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productid);

            if (product !=null)
            {
                GetCart().RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }


        private Cart GetCart()
        {
            Cart cart = (Cart) Session["Cart"];
            if (cart==null)
            {
                cart=new Cart();
                Session["Cart"] = cart;

            }

            return cart;
        }
        
    }
}