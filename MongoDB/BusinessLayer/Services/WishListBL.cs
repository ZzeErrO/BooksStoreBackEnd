using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class WishListBL : IWishListBL
    {
        IWishListRL wishListRL;
        public WishListBL(IWishListRL wishList)
        {
            this.wishListRL = wishList;
        }
        public dynamic GetWishList(string id)
        {
            try
            {
                return wishListRL.GetWishList(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Remove(string bookid, string userId)
        {
            try
            {
                this.wishListRL.Remove(bookid, userId);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
