﻿using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IUsersRL
    {
        public UserModel Authenticate(string email, string password);
    }
}
