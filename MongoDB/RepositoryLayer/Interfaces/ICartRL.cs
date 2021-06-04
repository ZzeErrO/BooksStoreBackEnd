using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRL
    {
        public Order Create(Order order);
        public dynamic GetCart(string id);
        public void Remove(string bookid, string userId);
    }
}
