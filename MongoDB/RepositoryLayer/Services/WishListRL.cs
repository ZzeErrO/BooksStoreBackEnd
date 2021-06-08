using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using MongoDB.Driver;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using MongoDB.Driver.Linq;
using System.Linq;

namespace RepositoryLayer.Services
{
    public class WishListRL : IWishListRL
    {
        private readonly IMongoCollection<UserModel> _users;
        private readonly IMongoCollection<WishList> _wishList;
        private readonly IMongoCollection<Cart> _cart;
        private readonly IMongoCollection<Book> _books;
        public WishListRL(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<UserModel>(settings.UsersCollectionName);
            _wishList = database.GetCollection<WishList>(settings.WishListCollectionName);
            _cart = database.GetCollection<Cart>(settings.CartCollectionName);
            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public dynamic GetWishList(string id)
        {
            try
            {

                var data = from c in _wishList.AsQueryable()
                           join b in _books.AsQueryable()
                           on c.BookId equals b.Id into result
                           where c.UserId == id
                           select new
                           {
                               BookName = result.First().BookName,
                               Price = result.First().Price,
                               Author = result.First().Authors,
                               BookId = c.BookId,
                               WishId = c.Id,
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
            this._wishList.DeleteOne(book => book.BookId == id && book.UserId == userId);
    }
}
