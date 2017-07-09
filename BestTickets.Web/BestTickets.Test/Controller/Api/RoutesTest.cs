using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BestTickets.Controllers;
using BestTickets.Domain.Abstractions;
using System.Linq;
using Moq;
using BestTickets.Domain.Models;

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
                new RouteRequest() {Id = 1, Route = new Route("Брест","Минск", null), RequestsCount = 20},
                new RouteRequest() {Id = 2, Route = new Route("Минск","Молодечно", null), RequestsCount = 17},
                new RouteRequest() {Id = 3, Route = new Route("Гродно","Витебск",null), RequestsCount = 3},
                new RouteRequest() {Id = 4, Route = new Route("Брест","Ольшаны",null), RequestsCount = 1},
                new RouteRequest() {Id = 5, Route = new Route("Москва","Минск",null), RequestsCount = 10},
                new RouteRequest() {Id = 6, Route = new Route("Кобрин","Брест",null), RequestsCount = 660},
                new RouteRequest() {Id = 7, Route = new Route("Гомель","Могилев",null), RequestsCount = 70},
                new RouteRequest() {Id = 8, Route = new Route("Москва","Брест",null), RequestsCount = 10},
                new RouteRequest() {Id = 9, Route = new Route("Пинск","Брест",null), RequestsCount = 50},
                new RouteRequest() {Id = 10, Route = new Route("Брест","Минск",null), RequestsCount = 10},
                new RouteRequest() {Id = 11, Route = new Route("Брест","Кобрин",null), RequestsCount = 5}
            };
            return requestedRoutes;
        }
    }
}
