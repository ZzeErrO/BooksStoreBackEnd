using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface IWishListBL
    {
        public dynamic GetWishList(string id);

        public void Remove(string bookid, string userId);

    }
}
