using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonLayer.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class CartRL : ICartRL
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly IMongoCollection<Cart> _cart;
        private readonly IMongoCollection<Book> _books;
        public CartRL(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _orders = database.GetCollection<Order>(settings.OrderCollectionName);
            _cart = database.GetCollection<Cart>(settings.CartCollectionName);
            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }
        public Order Create(Order order)
        {
            this._orders.InsertOne(order);
            return order;
        }

        public dynamic GetCart(string id)
        {
            try
            {
                /*var data = from b in _books.AsQueryable()
                           join c in _cart.AsQueryable()
                           on b.Id equals c.BookId into result
                           where result.First().UserId == id
                           select new
                           {
                               BookId = b.Id,
                               bookName = b.BookName,
                               AvailableBooks = b.AvailableBooks,
                               Price = b.Price,
                               Category = b.Category,
                               Author = b.Authors,
                               Arrival = b.Arrival,
                               Quantity = result.First().Quantity,
                           };*/

                var data = from c in _cart.AsQueryable()
                           join b in _books.AsQueryable()
                           on c.BookId equals b.Id into result
                           where c.UserId == id
                           select new
                           {
                                BookName = result.First().BookName,
                                Price = result.First().Price,
                                Author = result.First().Authors,
                                Quantity = c.Quantity,
                                BookId = c.BookId,
                                CartId = c.Id,
                                Email = c.Email
                           };

                //return this._cart.Find<Cart>(list => list.UserId == id).ToList();
                return data.ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Remove(string id, string userId) =>
            this._cart.DeleteOne(book => book.BookId == id && book.UserId == userId);

    }
}
