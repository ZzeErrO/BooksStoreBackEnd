using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class UsersBL : IUsersBL
    {
        IUsersRL usersRL;
        public UsersBL(IUsersRL userRL)
        {
            this.usersRL = userRL;
        }

        public UserModel Authenticate(string email, string password)
        {
            try
            {
                return usersRL.Authenticate(email, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UserModel Get(string id)
        {
            try
            {
                return usersRL.Get(id);
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
                return usersRL.ToWishList(book, userModel);
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
                return usersRL.ToCart(book, userModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
