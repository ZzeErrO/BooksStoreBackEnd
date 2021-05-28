using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WishListController : Controller
    {
        private readonly IBooksBL _bookService;
        private readonly IWishListBL _wishListBL;
        private readonly IUsersBL _usersBL;

        public WishListController(IUsersBL dataRepository, IBooksBL bookService, IWishListBL wishListBL)
        {
            _bookService = bookService;
            _wishListBL = wishListBL;
            _usersBL = dataRepository;
        }

        private List<string> GetTokenType()
        {
            string id = User.FindFirst("Id").Value;
            string type = User.FindFirst("ServiceType").Value;
            List<string> l = new List<string>();
            l.Add(id);
            l.Add(type);
            return l;
        }
        private UserModel Get(string id)
        {
            var user = _usersBL.Get(id);
            return user;
        }

        [HttpGet]
        public IActionResult GetWishListBooks()
        {
            if (GetTokenType()[1] != "Users")
            {

                return this.BadRequest(new { success = false, message = "Only Users Allowed" });
            }

            var wishList = _wishListBL.GetWishList(GetTokenType()[0]);
            return this.Ok(new {success= true, wishList });
        }

        [HttpPut("{id}/MoveToCart")]
        public IActionResult MoveToCart(string id)
        {
            if (GetTokenType()[1] != "Users")
            {

                return this.BadRequest(new { success = false, message = "Only Users Allowed" });
            }

            var book = _bookService.Get(id);

            if (book == null)
            {
                return this.BadRequest(new { success = false, message = "No such Book Exist" });
            }

            _usersBL.ToCart(book, Get(GetTokenType()[0]));

            return this.Ok(new { success = true, message = "Moved to Cart" });
        }
    }
}
