using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;

namespace RepositoryLayer.Interfaces
{
    public interface IBooksRL
    {
        public List<Book> Get();
        public List<Book> GetWishListBooks();
        public List<Book> GetCartBooks();
        public Book Get(string id);
        public Book Create(Book book);
        public void Update(string id, Book bookIn);
        public void Remove(Book bookIn);
        public void Remove(string id);
    }
}
