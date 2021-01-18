using System;
using Xunit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Core.Domain.Test
{
    public class CustomerTest
    {
        [Fact]
        public void Passed_If_Customer_Has_All_Required_Values()
        {
            // Arrange
            var name = "Joris Wessels";
            var emailAddress = "JorisWessels@omega.lol";
            var password = "NothingToSee123";
            
            // Act
            var customer = new Customer
            {
                Name = name,
                EmailAddress = emailAddress,
                Password = password
            };
            
            // Assert
            Assert.Equal(name, customer.Name);
            Assert.Equal(emailAddress, customer.EmailAddress);
            Assert.Equal(password, customer.Password);

        }
        
        [Fact]
        public void Fails_If_Customer_EmailAddress_Is_Not_Valid_Format()
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
