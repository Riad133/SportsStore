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
            Mock<IProductRepository> mock= new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1",Category = "cat1"},
                new Product {ProductID = 2, Name = "P2",Category = "cat2"},
                new Product {ProductID = 3, Name = "P3",Category = "cat1"},
                new Product {ProductID = 4, Name = "P4",Category = "cat2"},
                new Product {ProductID = 5, Name = "P5",Category = "cat3"}
            });


            ProductController controller= new ProductController(mock.Object);
            controller.PageSize = 3;
            //ACT



            Product[] result =( (ProductsListViewModel)controller.List(null, 1).Model).Products.ToArray();

           
            Assert.AreEqual(result.Length,3);
            Assert.IsTrue(result[0].Name=="P1" && result[0].Category=="cat1");
            Assert.IsTrue(result[1].Name== "P2" && result[1].Category=="cat2");


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
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"+ @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"+ @"<a class=""btn btn-default"" href=""Page3"">3</a>",result.ToString());
        }
    }
}
