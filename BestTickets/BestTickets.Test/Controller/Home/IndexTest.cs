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
        public void IndexIsNotNull()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexReturnIndexView()
        {
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
