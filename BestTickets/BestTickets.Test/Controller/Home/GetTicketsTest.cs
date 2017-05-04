using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestTickets.Controllers;
using System.Web.Mvc;
using BestTickets.Models;
using Moq;
using BestTickets.Infrastructure;
using System.Collections.Generic;

namespace BestTickets.Tests.Controller.Home
{
    [TestClass]
    public class GetTicketsTest
    {
        private HomeController controller;
        private PartialViewResult result;
        private RouteViewModel route;

        [TestInitialize]
        public void InitialSetups()
        {
            var mock = new Mock<IRepository<RouteRequest>>();
            mock.Setup(x => x.GetAll()).Returns(new List<RouteRequest>());
            controller = new HomeController(mock.Object);
            route = new RouteViewModel();
            route.DeparturePlace = "Минск";
            route.ArrivalPlace = "Молодечно";
            route.Date = route.SetCurrentDate();
            result = controller.GetTickets(route) as PartialViewResult;
        }

        [TestMethod]
        public void GetTicketsViewModelIsNotNull()
        {
            Assert.IsNotNull(result.Model);
        }

        [TestMethod]
        public void GetTicketsReturnPartialTicketsNotFoundViewIfModelEmpty()
        {
            var unknownRoute = new RouteViewModel("Nothing", "Nothing", route.SetCurrentDate());
            PartialViewResult unknownRouteResult = controller.GetTickets(unknownRoute) as PartialViewResult;
            Assert.AreEqual("_TicketsNotFound", unknownRouteResult.ViewName);
        }

        [TestMethod]
        public void GetTicketsReturnGetTicketsPartialViewIfModelNotEmpty()
        {
            Assert.AreEqual("_GetTickets", result.ViewName);
        }

    }
}
