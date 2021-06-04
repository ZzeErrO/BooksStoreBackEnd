using System;
using System.Collections.Generic;
using System.Text;
using BusinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Interfaces;

namespace BusinessLayer.Services
{
    public class CartBL : ICartBL
    {
        readonly ICartRL cartRL;
        public CartBL(ICartRL cartRL)
        {
            this.cartRL = cartRL;
        }
        public Order Create(Order order)
        {
            return this.cartRL.Create(order);
        }
        public dynamic GetCart(string bookid)
        {
            try
            {
                return this.cartRL.GetCart(bookid);
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
                this.cartRL.Remove(bookid, userId);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}