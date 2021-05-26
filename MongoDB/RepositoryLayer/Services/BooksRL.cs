using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.Models;
using MongoDB.Driver;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace RepositoryLayer.Services
{
    public class BooksRL : IBooksRL
    {
        private readonly IMongoCollection<Book> _books;
        public BooksRL(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }
        public List<Book> Get() =>
            this._books.Find(book => true).ToList();

        public Book Get(string id) =>
            this._books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            this._books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            this._books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            this._books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            this._books.DeleteOne(book => book.Id == id);
    }
}
