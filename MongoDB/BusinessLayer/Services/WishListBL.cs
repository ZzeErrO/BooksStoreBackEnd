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
        public List<WishList> GetWishList(string id)
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
    }
}
