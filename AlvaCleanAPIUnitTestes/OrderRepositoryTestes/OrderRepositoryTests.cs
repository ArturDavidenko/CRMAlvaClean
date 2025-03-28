using AlvaCleanAPI.DataContext;
using AlvaCleanAPI.Models;
using AlvaCleanAPI.Repository;
using AlvaCleanAPI.Repository.Interfaces;
using FakeItEasy;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlvaCleanAPIUnitTestes.OrderRepositoryTestes
{
    public class OrderRepositoryTests
    {
        private readonly IMongoCollection<Order> _ordersCollection;
        private readonly IMongoCollection<Customer> _customersCollection;
        private readonly MongoContext _mongoContext;
        private readonly IOptions<MongoSettings> _mongoSettings;
        private readonly OrderRepository _orderRepository;

        public OrderRepositoryTests()
        {
            _ordersCollection = A.Fake<IMongoCollection<Order>>();
            _customersCollection = A.Fake<IMongoCollection<Customer>>();

            _mongoContext = A.Fake<MongoContext>();

            A.CallTo(() => _mongoContext.Orders).Returns(_ordersCollection);
            A.CallTo(() => _mongoContext.Customers).Returns(_customersCollection);

            // Создаём моки для настроек MongoDB
            var fakeSettings = new MongoSettings { MongoUrl = "mongodb://localhost:27017", MongoDbName = "TestDb" };
            _mongoSettings = Options.Create(fakeSettings);

            //Error: The connection string '' is not valid.
            _orderRepository = new OrderRepository(_mongoSettings);
            typeof(OrderRepository)
                .GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_orderRepository, _mongoContext);
            _orderRepository = new OrderRepository(_mongoSettings);
        }


        [Fact]
        public async Task CreateOrder_Should_Insert_Order_Into_Database()
        {
            //Arrange

            var orderModel = new RegisterOrderModel
            {
                OrderType = "Test Type",
                OrderStartDate = System.DateTime.UtcNow,
                Status = "New",
                Address = "123 Street",
                OrderPriceType = "Fixed",
                ClientComments = "Test Comment",
                Area = 100,
                Hour = 5,
                CustomerName = "John Doe",
                Price = 150
            };

            var customerId = ObjectId.GenerateNewId().ToString();


            //Act

            await _orderRepository.CreateOrder(orderModel, customerId);

            //Assert 

            A.CallTo(() => _ordersCollection.InsertOne(A<Order>._, A<InsertOneOptions>._, A<System.Threading.CancellationToken>._))
            .MustHaveHappenedOnceExactly();
        }




    }
}
