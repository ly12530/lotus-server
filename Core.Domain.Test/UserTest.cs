using System;
using Xunit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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

        [Fact]
        public void Member_Should_Create_The_Right_Way()
        {
            // Arrange
            var id = 1;
            var userName = "Joris";
            var emailAddress = "JorisWessels@omega.lol";
            var role = Role.Member;
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
            Assert.Equal(Role.Member, user.Role);
            Assert.Equal(password, user.Password);
            Assert.Equal(requests, user.Requests);
            Assert.Equal(jobs, user.Jobs);
        }

        [Fact]
        public void Administrator_Should_Create_The_Right_Way()
        {
            // Arrange
            var id = 1;
            var userName = "Joris";
            var emailAddress = "JorisWessels@omega.lol";
            var role = Role.Administrator;
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
            Assert.Equal(Role.Administrator, user.Role);
            Assert.Equal(password, user.Password);
            Assert.Equal(requests, user.Requests);
            Assert.Equal(jobs, user.Jobs);
        }

        [Fact]
        public void Customer_Should_Create_The_Right_Way()
        {
            // Arrange
            var id = 1;
            var userName = "Joris";
            var emailAddress = "JorisWessels@omega.lol";
            var role = Role.Customer;
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
            Assert.Equal(Role.Customer, user.Role);
            Assert.Equal(password, user.Password);
            Assert.Equal(requests, user.Requests);
            Assert.Equal(jobs, user.Jobs);
        }

        [Fact]
        public void Instructor_Should_Create_The_Right_Way()
        {
            // Arrange
            var id = 1;
            var userName = "Joris";
            var emailAddress = "JorisWessels@omega.lol";
            var role = Role.Member;
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

            user.Role = Role.Instructor;

            // Assert
            Assert.Equal(id, user.Id);
            Assert.Equal(userName, user.UserName);
            Assert.Equal(emailAddress, user.EmailAddress);
            Assert.Equal(Role.Instructor, user.Role);
            Assert.Equal(password, user.Password);
            Assert.Equal(requests, user.Requests);
            Assert.Equal(jobs, user.Jobs);
        }

        [Fact]
        public void Fails_If_EmailAddress_Is_Not_Valid_Format()
        {
            // Arrange
            var emailAddress = "JorisWessels_omega.lol,dasda";

            var thrownException = false;

            // Act
            try
            {
                var regexResult = Regex.Match(emailAddress, "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*").Success;

                if (!regexResult) throw new InvalidOperationException();
            }
            catch
            {
                thrownException = true;
            }
            
            // Assert
            Assert.True(thrownException);
        }
    }
}
