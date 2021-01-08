using System;
using Xunit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Test
{
    public class UserTest
    {
        [Fact]
        public void User_Should_Create_The_Right_Way()
        {
            // Arrange
            var id = 1;
            var userName = "Joris";
            var emailAddress = "JorisWessels@omega.lol";
            var role = new Role();
            var password = "JorikIsCool";
            var requests = new HashSet<Request>();
            var jobs = new HashSet<Request>();

            // Act
            var user = new User
            {
                Id = id,
                UserName = userName,
                EmailAddress = emailAddress,
                Role = role,
                Password = password,
                Requests = requests,
                Jobs = jobs

            };

            // Assert
            Assert.Equal(id, user.Id);
            Assert.Equal(userName, user.UserName);
            Assert.Equal(emailAddress, user.EmailAddress);
            Assert.Equal(role, user.Role);
            Assert.Equal(password, user.Password);
            Assert.Equal(requests, user.Requests);
            Assert.Equal(jobs, user.Jobs);
        }
    }
}
