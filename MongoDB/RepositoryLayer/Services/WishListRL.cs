using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using MongoDB.Driver;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class WishListRL : IWishListRL
    {
        private readonly IMongoCollection<UserModel> _users;
        private readonly IMongoCollection<WishList> _wishList;
        private readonly IMongoCollection<Cart> _cart;
        public WishListRL(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<UserModel>(settings.UsersCollectionName);
            _wishList = database.GetCollection<WishList>(settings.WishListCollectionName);
            _cart = database.GetCollection<Cart>(settings.CartCollectionName);
        }

        public List<WishList> GetWishList(string id) =>
                this._wishList.Find<WishList>(list => list.UserId == id).ToList();
    }
}
