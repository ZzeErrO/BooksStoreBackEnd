using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class BookstoreDatabaseSettings : IBookstoreDatabaseSettings
    {
        public string BooksCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
        public string AdminCollectionName { get; set; }
        public string WishListCollectionName { get; set; }
        public string CartCollectionName { get; set; }
        public string OrderCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IBookstoreDatabaseSettings
    {
        string BooksCollectionName { get; set; }
        string UsersCollectionName { get; set; }
        string AdminCollectionName { get; set; }
        string WishListCollectionName { get; set; }
        string CartCollectionName { get; set; }
        string OrderCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
