using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface IWishListBL
    {
        public List<WishList> GetWishList(string id);
    }
}
