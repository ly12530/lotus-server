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
