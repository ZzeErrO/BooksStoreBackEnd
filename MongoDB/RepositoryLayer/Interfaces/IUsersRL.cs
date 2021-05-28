using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IUsersRL
    {
        public UserModel Authenticate(string email, string password);

        public UserModel Get(string id);

        public WishList ToWishList(Book book, UserModel userModel);

        public Cart ToCart(Book book, UserModel userModel);
    }
}
