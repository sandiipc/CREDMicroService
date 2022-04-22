using CREDMicroService.Database;
using CREDMicroService.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CREDMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        DatabaseContext db;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DatabaseContext(_configuration);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult Login([FromBody] Credential credential)
        {
            var user = Authenticate(credential);

            if(user != null)
            {
                var token = GenerateToken(user);

                //return Ok(token);
                return new JsonResult(token);

            }

            //return NotFound("User not found");
            return new JsonResult("User not found");


        }

        private string GenerateToken(UserLogin user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.GivenName, user.GivenName),
                new Claim(ClaimTypes.Surname, user.Surname),
                new Claim(ClaimTypes.Role, user.Role)

            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims,
                expires: DateTime.Now.AddMinutes(15), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        private UserLogin Authenticate(Credential credential)
        {
            var currentUser = db.UserLogins.Where(u => u.Username.ToLower() == credential.Username.ToLower() && 
            u.Password == credential.Password).FirstOrDefault();

            if(currentUser != null)
            {
                return currentUser;
            }

            return null;


        }



    }
}
