using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "cat3"}
            });


            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            //ACT



            Product[] result = ((ProductsListViewModel) target.List(null, 1).Model).Products.ToArray();


            Assert.AreEqual(result.Length, 3);
            Assert.IsTrue(result[0].Name == "P1" && result[0].Category == "cat1");
            Assert.IsTrue(result[1].Name == "P2" && result[1].Category == "cat2");


        }


        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;
            // Arrange - create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            // Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);


            // Assert
            Assert.AreEqual(
                @"<a class=""btn btn-default"" href=""Page1"">1</a>" +
                @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" +
                @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }



        [TestMethod]
        public void Generate_Category_Specification_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "cat3"}
            });
            //Arrange -- create a controller and make page 
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            //Action -- test the product counts for different categories
            int res1 = ((ProductsListViewModel) target.List("cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel) target.List("cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel) target.List("cat3").Model).PagingInfo.TotalItems;

            int resAll = ((ProductsListViewModel) target.List(null).Model).PagingInfo.TotalItems;

            
            //Assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);

        }

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            Product p1= new Product{ProductID = 1, Name = "p1"};
            Product p2 = new Product { ProductID = 2, Name = "p2" };




            //Arrange
            Cart target= new Cart();
            //act

            target.AddItem(p1,1);
            target.AddItem(p2,3);
            CartLine[] result = target.Lines.ToArray();
            //Assert
            Assert.AreEqual(result.Length,2);
            Assert.AreEqual(result[0].Product,p1);
            Assert.AreEqual(result[0].Quantity,1);
            Assert.AreEqual(result[1].Quantity,3);
        
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            Product p1 = new Product { ProductID = 1, Name = "p1", Price = 14};
            Product p2 = new Product { ProductID = 2, Name = "p2", Price = 10};

            Product p3 = new Product { ProductID = 3, Name = "p3", Price = 18};



            //Arrange
            Cart target = new Cart();
            //act

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3,2);
           target.RemoveLine(p2);
           // target.Clear();
            decimal total = target.ComputeTotalValue();

            //assert
            Assert.AreEqual(total,50);
            Assert.AreEqual(target.Lines.Count(x => x.Product==p2 ),0);
            Assert.AreEqual(target.Lines.Count(),2);
            
        }

       
        


    }
}
