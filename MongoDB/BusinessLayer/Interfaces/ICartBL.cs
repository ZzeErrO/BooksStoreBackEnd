using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface ICartBL
    {
        public Order Create(Order order);

        public dynamic GetCart(string id);

        public void Remove(string bookid, string userId);
    }
}
