using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface IBooksBL
    {
        public List<Book> Get();
        public Book Get(string id);
        public Book Create(Book book);
        public void Update(string id, Book bookIn);
        public void Remove(Book bookIn);
        public void Remove(string id);
    }
}
