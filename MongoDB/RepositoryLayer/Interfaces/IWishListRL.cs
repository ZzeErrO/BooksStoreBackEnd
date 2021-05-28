using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IWishListRL
    {
        public List<WishList> GetWishList(string id);
    }
}
