using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestTickets.Controllers;
using System.Web.Mvc;

namespace BestTickets.Tests.Controller.Home
{
    [TestClass]
    public class IndexTest
    {
        private HomeController controller;
        private ViewResult result;

        [TestInitialize]
        public void InitialSetups()
        {
            controller = new HomeController();
            result = controller.Index() as ViewResult;
        }

        [TestMethod]
        public void Index_IsNotNull()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Index_ReturnIndexView()
        {
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
