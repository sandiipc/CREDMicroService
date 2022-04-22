using CREDMicroService.Database;
using CREDMicroService.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CREDMicroService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IConfiguration _configuration;
        DatabaseContext db;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            db = new DatabaseContext(_configuration);
            
        }

        // GET: api/<UserController>
        [HttpGet]
        [Authorize]
        public IEnumerable<User> Get()
        {
            return db.Users.ToList();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [Authorize]
        public User Get(int id)
        {
            var user = db.Users.Find(id);
            if(user != null)
                return db.Users.Find(id);

            var emptyUser = new User();
            return emptyUser;

        }

        // POST api/<UserController>
        [HttpPost]
        [Route("create")]
        [Authorize]
        public IActionResult Post([FromBody] User model)
        {
            try
            {
                db.Users.Add(model);
                db.SaveChanges();

                return Created("~api/user/created", new { statusCode = StatusCodes.Status201Created, responseObject = model });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = StatusCodes.Status500InternalServerError, errorMsg = ex.Message });
            }


        }

        // POST api/<UserController>/5
        [HttpPost]
        [Route("edit/{id}")]
        [Authorize]
        public IActionResult Post(int id, [FromBody] UpdatedUser model)
        {
            try
            {
                var user = db.Users.Find(id);
                if(user != null)
                {
                    user.Address = model.Address;
                    user.Contact = model.Contact;

                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { statusCode = StatusCodes.Status200OK, responseObject = model });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { statusCode = StatusCodes.Status404NotFound, message = "User not found" });
                }
                

            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = StatusCodes.Status500InternalServerError, errorMsg = ex.Message });
            }

        }

        // GET api/<UserController>/5
        [HttpGet]
        [Route("delete/{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {

            try
            {
                var user = db.Users.Find(id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();

                    return StatusCode(StatusCodes.Status200OK, new { statusCode = StatusCodes.Status200OK, message = "User successfully deleted" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new { statusCode = StatusCodes.Status404NotFound, message = "User not found" });
                }


            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { statusCode = StatusCodes.Status500InternalServerError, errorMsg = ex.Message });
            }

        }


        //method to get current user
        private UserLogin GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if(identity != null)
            {
                var userClaims = identity.Claims;

                return new UserLogin
                {
                    Username = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value,
                    EmailAddress = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value,
                    GivenName = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName).Value,
                    Surname = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Surname).Value,
                    Role = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value

                };

            }

            return null;

        }



    }
}
