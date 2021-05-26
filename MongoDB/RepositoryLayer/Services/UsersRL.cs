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
        public UsersRL(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<UserModel>(settings.UsersCollectionName);
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
    }
}
