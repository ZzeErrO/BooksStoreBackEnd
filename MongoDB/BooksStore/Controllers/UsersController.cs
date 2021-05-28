using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BooksStore.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersBL _usersBL;
        private readonly IBooksBL _bookService;

        private readonly string _secret;
        private readonly string _issuer;
        public UsersController(IUsersBL dataRepository, IConfiguration config, IBooksBL bookService)
        {
            _usersBL = dataRepository;
            _bookService = bookService;
            _secret = config.GetSection("Jwt").GetSection("Key").Value;
            _issuer = config.GetSection("Jwt").GetSection("Issuer").Value;
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

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Authenticate([FromBody] LoginRequestModel model)
        {
            try
            {
                var user = _usersBL.Authenticate(model.Email, model.Password);

                if (user == null)
                    return BadRequest(new { message = "Password is incorrect" });

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Issuer = _issuer,
                    Audience = _issuer,
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("Id", Convert.ToString(user.Id)),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim("ServiceType", "Users"),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(1440),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                return Ok(new
                {   
                    success = true,
                    message = "Login Successfull",
                    Id = user.Id,
                    Email = user.Email,
                    Token = tokenString
                });
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { success = false, message = ex.Message });
            }

        }

        [HttpGet]
        public IActionResult Get()
        {
            var books = _bookService.Get();
            return this.Ok(new {success= true, books });
        }

        [HttpPut("{id}/MoveToWishList")]
        public IActionResult MoveToWishList(string id)
        {
            if (GetTokenType()[1] != "Users")
            {

                return this.BadRequest(new { success = false, message = "Only Users Allowed" });
            }

            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            var wish = _usersBL.ToWishList(book, Get(GetTokenType()[0]));

            return this.Ok(new { success = true, message = "Moved to Wish List" });
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
                return NotFound();
            }

            _usersBL.ToCart(book, Get(GetTokenType()[0]));

            return this.Ok(new { success = true, message = "Moved to Cart" });
        }
    }
}
