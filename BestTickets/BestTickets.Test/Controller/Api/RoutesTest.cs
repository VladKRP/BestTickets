using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestTickets.Models;
using BestTickets.Controllers;
using BestTickets.Infrastructure;
using System.Linq;
using Moq;

namespace BestTickets.Tests.Controller.Api
{

    [TestClass]
    public class RoutesTest
    {
        private IEnumerable<RouteRequest> testItems;
        private Mock<IRouteRequestRepository> mockRepository;
        private RoutesController controller;

        [TestInitialize]
        public void InitialSetups()
        {
            testItems = GetRoutes();
            mockRepository = new Mock<IRouteRequestRepository>();
            mockRepository.Setup(x => x.GetTop10()).Returns(testItems.OrderByDescending(x => x.RequestsCount).Take(10).AsQueryable());
            controller = new RoutesController(mockRepository.Object);
        }

        [TestMethod]
        public void GetTop10Routes_IsNotNull()
        {
            var result = controller.GetTop10Routes();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetTop10Routes_Return10Routes()
        {
            const int expectedItemsQuantity = 10;
            var result = controller.GetTop10Routes();
            Assert.AreEqual(expectedItemsQuantity, result.Count());
        }

        [TestMethod]
        public void GetTop10Routes_ReturnRightRoutes()
        {
            var expectedFirstRoute = testItems.FirstOrDefault(x => x.Id.Equals(6)).Route;
            var result = controller.GetTop10Routes();
            var actualFirstRoute = result.First();
            Assert.AreEqual(expectedFirstRoute, actualFirstRoute);
        }


        private IEnumerable<RouteRequest> GetRoutes()
        {

            var requestedRoutes = new List<RouteRequest>()
            {
                new RouteRequest() {Id = 1, Route = new RouteViewModel("Брест","Минск",""), RequestsCount = 20},
                new RouteRequest() {Id = 2, Route = new RouteViewModel("Минск","Молодечно",""), RequestsCount = 17},
                new RouteRequest() {Id = 3, Route = new RouteViewModel("Гродно","Витебск",""), RequestsCount = 3},
                new RouteRequest() {Id = 4, Route = new RouteViewModel("Брест","Ольшаны",""), RequestsCount = 1},
                new RouteRequest() {Id = 5, Route = new RouteViewModel("Москва","Минск",""), RequestsCount = 10},
                new RouteRequest() {Id = 6, Route = new RouteViewModel("Кобрин","Брест",""), RequestsCount = 660},
                new RouteRequest() {Id = 7, Route = new RouteViewModel("Гомель","Могилев",""), RequestsCount = 70},
                new RouteRequest() {Id = 8, Route = new RouteViewModel("Москва","Брест",""), RequestsCount = 10},
                new RouteRequest() {Id = 9, Route = new RouteViewModel("Пинск","Брест",""), RequestsCount = 50},
                new RouteRequest() {Id = 10, Route = new RouteViewModel("Брест","Минск",""), RequestsCount = 10},
                new RouteRequest() {Id = 11, Route = new RouteViewModel("Брест","Кобрин",""), RequestsCount = 5}
            };
            return requestedRoutes;
        }
    }
}
