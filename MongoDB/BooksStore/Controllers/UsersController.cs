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
        //private readonly INotesBL _noteBL;
        private readonly string _secret;
        private readonly string _issuer;
        public UsersController(IUsersBL dataRepository, IConfiguration config)
        {
            _usersBL = dataRepository;
            _secret = config.GetSection("Jwt").GetSection("Key").Value;
            _issuer = config.GetSection("Jwt").GetSection("Issuer").Value;
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
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(1440),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                // return basic user info and authentication token
                return Ok(new
                {
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
    }
}
