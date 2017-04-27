using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestTickets.Controllers;
using System.Web.Mvc;
using BestTickets.Infrastructure;
using Moq;
using System.Collections.Generic;
using BestTickets.Models;

namespace BestTickets.Tests.Controller.Home
{
    [TestClass]
    public class GetTop10FrequentRequestsTest
    {
        //private Mock mock;
        private HomeController controller;
        private PartialViewResult result;

        [TestInitialize]
        public void InitialSetups()
        {
            var mock = new Mock<IRouteRequestRepository>();
            mock.Setup(x => x.GetAll()).Returns(new List<RouteRequest>());
            controller = new HomeController(mock.Object);
            result = controller.GetTop10MostFrequentRequests() as PartialViewResult;
        }

        [TestMethod]
        public void GetTop10FrequentRequestsIsNotNull()
        {
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTop10FrequentResultsReturnRightPartialView()
        {
            Assert.AreEqual("_Top10FrequentRequests", result.ViewName);
        }


    }
}
