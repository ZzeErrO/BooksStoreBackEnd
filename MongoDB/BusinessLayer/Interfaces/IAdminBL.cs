using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface IAdminBL
    {
        public UserModel Authenticate(string email, string password);
    }
}
