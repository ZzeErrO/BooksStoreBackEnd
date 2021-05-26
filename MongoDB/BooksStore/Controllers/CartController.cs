using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly ICartBL _cart;

        public  CartController(ICartBL bookService)
        {
            _cart = bookService;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string bookId, [FromBody] int quantity)
        {
            

            return NoContent();
        }
    }
}
