using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CREDMicroService.Database.Entities
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Role { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }


    }
}
