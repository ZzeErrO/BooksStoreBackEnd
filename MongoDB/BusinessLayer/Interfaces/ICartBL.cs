using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface ICartBL
    {
       public Order Create(Order order);
    }
}
