using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using MongoDB.Driver;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class CartRL : ICartRL
    {
        private readonly IMongoCollection<Order> _orders;
        public CartRL(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _orders = database.GetCollection<Order>(settings.OrderCollectionName);
        }
        public Order Create(Order order)
        {
            this._orders.InsertOne(order);
            return order;
        }

    }
}
