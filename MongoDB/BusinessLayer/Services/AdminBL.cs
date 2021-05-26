using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class AdminBL : IAdminBL
    {
        IAdminRL adminRL;
        public AdminBL(IAdminRL userRL)
        {
            this.adminRL = userRL;
        }

        public UserModel Authenticate(string email, string password)
        {
            try
            {
                return adminRL.Authenticate(email, password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
