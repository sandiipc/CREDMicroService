using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using CREDMicroService.Database.Entities;
using CREDMicroService.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace CREDMicroserviceXUnitTest
{
    public class UserControllerTest
    {
        private readonly Dictionary<string, string> inMemorySettings;
        private IConfiguration configuration;

        public UserControllerTest()
        {
            inMemorySettings = new Dictionary<string, string> {
                                        {"SQLConnString", GetConnectionString()}
                                    };

            configuration = new ConfigurationBuilder()
                                        .AddInMemoryCollection(inMemorySettings)
                                        .Build();

        }

        [Fact]
        public void GetUsers_ListOfUser_UserExistsInDB()
        {
            //arrange
            var users = GetSampleUser();
            var controller = new UserController(configuration);

            //act
            var actionResult = controller.Get();
            var actual = actionResult as List<User>;

            //assert
            Assert.Equal(users.Count, actual.Count);

        }


        [Fact]
        public void GetUser_ById_UserExistsInDB()
        {
            //arrange
            var users = GetSampleUser();
            var userById = users.Find(m => m.UserId == 1);
            var controller = new UserController(configuration);

            //act
            var actualUserById = controller.Get(1);

            //assert
            Assert.Equal(userById.Name, actualUserById.Name);

        }


        [Fact]
        public void CreateUser_DBInsert()
        {
            //arrange
            var user = new User
            {
                Name = "Pupet Maduro",
                Address = "VEN",
                Contact = "1234567890"
            };
            var controller = new UserController(configuration);

            //act
            var actionResult = controller.Post(user);
            var result = actionResult as ObjectResult;

            //assert
            Assert.Equal(201, result.StatusCode);

        }


        [Fact]
        public void EditUser_DBUpdate()
        {
            //arrange
            var updatedUser = new UpdatedUser
            {
                Address = "JPN",
                Contact = "0987654321"
            };
            var controller = new UserController(configuration);

            //act
            var actionResult = controller.Post(6, updatedUser);
            var result = actionResult as ObjectResult;

            //assert
            Assert.Equal(200, result.StatusCode);

        }

        [Fact]
        public void DeleteUser_DBRemove()
        {
            //arrange            
            var controller = new UserController(configuration);

            //act
            var actionResult = controller.Delete(7);
            var result = actionResult as ObjectResult;

            //assert
            Assert.Equal(200, result.StatusCode);

        }




        private string GetConnectionString()
        {
            string connStr = "Server=tcp:azsqlserverdatabase.database.windows.net,1433;Initial Catalog=microservicedb;Persist Security Info=False;User ID=azsa;Password=TcsP@ssw0rd@4;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            return connStr;
        }
        private List<User> GetSampleUser()
        {
            List<User> output = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Name = "Sandip",
                    Address = "RUS",
                    Contact = "1234567890"
                },
                new User
                {
                    UserId = 2,
                    Name = "Volodymir Jelensky",
                    Address = "UKR",
                    Contact = "1234567890"
                },
                new User
                {
                    UserId = 3,
                    Name = "Joe Biden",
                    Address = "US",
                    Contact = "1234567890"
                },
                new User
                {
                    UserId = 4,
                    Name = "Boris Johnson",
                    Address = "UK",
                    Contact = "1234567890"
                },
                new User
                {
                    UserId = 5,
                    Name = "Pupet Maduro",
                    Address = "VEN",
                    Contact = "1234567890"
                }

            };

            return output;


        }


    }
}
