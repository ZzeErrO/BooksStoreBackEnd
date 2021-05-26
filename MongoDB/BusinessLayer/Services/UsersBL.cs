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
    }
}
