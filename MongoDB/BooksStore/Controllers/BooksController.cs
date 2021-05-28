using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace BooksStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IBooksBL _bookService;

        private readonly IDistributedCache distributedCache;

        public BooksController(IBooksBL bookService, IDistributedCache distributedCache)
        {
            _bookService = bookService;

            this.distributedCache = distributedCache;
        }
        private List<string> GetTokenType()
        {
            string id = User.FindFirst("Id").Value;
            string type = User.FindFirst("ServiceType").Value;
            List<string> l = new List<string>();
            l.Add(id);
            l.Add(type);
            return l ;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var books = _bookService.Get();
            return this.Ok(new { success = true, books });
        }

        [AllowAnonymous]
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllBooksUsingRedisCache()
        {
            var cacheKey = "bookList";
            string serializedBookList;
            var bookList = new List<Book>();
            var redisCustomerList = await distributedCache.GetAsync(cacheKey);
            if (redisCustomerList != null)
            {
                serializedBookList = Encoding.UTF8.GetString(redisCustomerList);
                bookList = JsonConvert.DeserializeObject<List<Book>>(serializedBookList);
            }
            else
            {
                bookList = _bookService.Get();
                serializedBookList = JsonConvert.SerializeObject(bookList);
                redisCustomerList = Encoding.UTF8.GetBytes(serializedBookList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisCustomerList, options);
            }
            return Ok(bookList);
        }

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public IActionResult Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return this.BadRequest(new { success = false, message = "No Book Found" });
            }

            return this.Ok(new {success = true, book });
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            if (GetTokenType()[1] != "Admin")
            {

                return this.BadRequest(new { success = false, message = "Only Admin Allowed" });
            }

            _bookService.Create(book);

            return this.Ok(new { success = true, message = "Added new Book to data"});
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            if (GetTokenType()[1] != "Admin")
            {

                return this.BadRequest(new { success = false, message = "Only Admin Allowed" });
            }

            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return this.Ok(new { success = true, message = "Book Updated" });
        }


        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            if (GetTokenType()[1] != "Admin")
            {

                return this.BadRequest(new { success = false, message = "Only Admin Allowed" });
            }

            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);

            return this.Ok(new { success = true, message = "Book Deleted" });
        }
    }
}
