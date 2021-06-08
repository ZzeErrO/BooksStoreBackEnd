using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly IBooksBL _bookService;
        private readonly ICartBL _cartService;

        public CartController(IBooksBL bookService, ICartBL cartService)
        {
            _bookService = bookService;
            _cartService = cartService;
        }

        private List<string> GetTokenType()
        {
            string id = User.FindFirst("Id").Value;
            string type = User.FindFirst("ServiceType").Value;
            string email = User.FindFirst(ClaimTypes.Email).Value;
            List<string> l = new List<string>();
            l.Add(id);
            l.Add(type);
            l.Add(email);
            return l;
        }

        [HttpGet]
        public IActionResult GetCartBooks()
        {
            try
            {
                if (GetTokenType()[1] != "Users")
                {

                    return this.BadRequest(new { success = false, message = "Only Users Allowed" });
                }

                var book = _cartService.GetCart(GetTokenType()[0]);
                var books = _bookService.Get();
                return this.Ok(new { success = true, book });
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("{_bookid}")]
        public IActionResult Delete(string _bookid)
        {
            try
            {
                if (GetTokenType()[1] != "Users")
                {

                    return this.BadRequest(new { success = false, message = "Only Users Allowed" });
                }

                var cartBook = _cartService.GetCart(GetTokenType()[0]);

                if (cartBook == null)
                {
                    return NotFound();
                }


                _cartService.Remove(_bookid, GetTokenType()[0]);

                return this.Ok(new { success = true, message = "Book Deleted" });
            }
            catch(Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex });
            }
        }

        [HttpPut("Order/{bookId}/{quantity}")]
        public IActionResult OrderBook(string bookId, int quantity)
        {
            if (GetTokenType()[1] != "Users")
            {
                return this.BadRequest(new { success = false, message = "Only Users Allowed" });
            }

            var cartBook = _cartService.GetCart(GetTokenType()[0]);
            var book = _bookService.Get(bookId);

            if (cartBook == null)
            {
                return this.BadRequest(new { success = false, message = "Book is not in Cart" });
            }

            if (book == null)
            {
                return this.BadRequest(new { success = false, message = "Book does not exist anymore in Store" });
            }

            book.AvailableBooks = book.AvailableBooks - quantity;

            if (book.AvailableBooks >= 0)
            {
                _bookService.Update(bookId, book);
                _cartService.Remove(bookId, GetTokenType()[0]);
                Order order = new Order() { BookName = book.BookName, Price = book.Price, Email = GetTokenType()[2], BooksOrdered = quantity};
                _cartService.Create(order);
                return this.Ok(new { success = true, message = "Book Ordered" });
            }
            else
            {
                return this.BadRequest(new { success = false, message = "Book Order Failed due to Availablity" });
            }
        }
    }
}
