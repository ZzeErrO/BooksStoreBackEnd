using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IWishListRL
    {
        public dynamic GetWishList(string id);

        public void Remove(string bookid, string userId);
    }
}
