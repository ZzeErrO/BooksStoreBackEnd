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
        private readonly IMongoCollection<Cart> _cart;
        public CartRL(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _orders = database.GetCollection<Order>(settings.OrderCollectionName);
            _cart = database.GetCollection<Cart>(settings.CartCollectionName);
        }
        public Order Create(Order order)
        {
            this._orders.InsertOne(order);
            return order;
        }

        public List<Cart> GetCart(string id) =>
            this._cart.Find<Cart>(list => list.UserId == id).ToList();

        public void Remove(string id, string userId) =>
            this._cart.DeleteOne(book => book.BookId == id && book.UserId == userId);

    }
}
