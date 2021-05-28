using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer;
using CommonLayer.Models;
using MongoDB.Driver;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class UsersRL : IUsersRL
    {
        private readonly IMongoCollection<UserModel> _users;
        private readonly IMongoCollection<WishList> _wishList;
        private readonly IMongoCollection<Cart> _cart;
        public UsersRL(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<UserModel>(settings.UsersCollectionName);
            _wishList = database.GetCollection<WishList>(settings.WishListCollectionName);
            _cart = database.GetCollection<Cart>(settings.CartCollectionName);
        }

        public UserModel Authenticate(string email, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return null;
          
                var user = this._users.Find<UserModel>(UserModel => UserModel.Email == email).FirstOrDefault();

                // check if password is correct
                if (password != user.Password)
                    return null;
                
                // check if email exists
                if (user == null)
                {
                    throw new BookStoreCustomExceptions(BookStoreCustomExceptions.ExceptionType.INVALID_EMAIL, "Email is Invalid");
                }

                // authentication successful
                return user;
            }
            catch (Exception ex)
            {
                throw new BookStoreCustomExceptions(BookStoreCustomExceptions.ExceptionType.INVALID_EMAIL, "Email is Invalid");
            }
        }

        public UserModel Get(string id)
        {
            try
            {
                return this._users.Find<UserModel>(user => user.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public WishList ToWishList(Book book, UserModel userModel)
        {
            try
            {
                WishList wishlist = new WishList() { BookId = book.Id, Email = userModel.Email, Price = book.Price, UserId = userModel.Id };
                this._wishList.InsertOne(wishlist);
                return wishlist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Cart ToCart(Book book, UserModel userModel)
        {
            try
            {
                Cart cart = new Cart() { BookId = book.Id, Email = userModel.Email, Price = book.Price, Quantity = 1, UserId = userModel.Id };
                this._cart.InsertOne(cart);
                this._wishList.DeleteOne(userandbook => userandbook.UserId == userModel.Id && userandbook.BookId == book.Id);
                return cart;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
