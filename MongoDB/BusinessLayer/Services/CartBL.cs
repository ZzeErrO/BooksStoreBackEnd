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
    }
}
