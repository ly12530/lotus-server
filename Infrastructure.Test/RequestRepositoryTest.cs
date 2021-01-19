using Core.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Infrastructure.Test
{
    public class RequestRepositoryTest
    {
        [Fact]
        public async void Get_All_Requests()
        {
            //Arrange
            var mockSet = new Mock<DbSet<Request>>();
            var mockContext = new Mock<LotusDbContext>();
            mockContext.Setup(r => r.Requests).Returns(mockSet.Object);
            var sut = new RequestRepository(mockContext.Object) {  };
            foreach (var request in requestsList)
            {
                await sut.AddRequest(request);
            }

            //Act
            var result = sut.GetAllRequests();

            //Assert
            Assert.Equal(11, result.Count());
           
        }
        
        List<Request> requestsList = new List<Request>
         {
                new Request{ DesignatedUser = new User()},
                new Request{ },
                new Request{ IsOpen = false},
                new Request{ IsOpen = true},
                new Request{ DesignatedUser = new User(), IsOpen = true},
                new Request{ DesignatedUser = new User(), IsOpen = false},
                new Request{ Date = new DateTime() },
                new Request{ DesignatedUser = new User(), Date = new DateTime()},
                new Request{ DesignatedUser = new User(), Date = new DateTime(), IsOpen = true},
                new Request{ DesignatedUser = new User(), Date = new DateTime(), IsOpen = false},
                new Request{ Date = new DateTime(), IsOpen = true},
         };

    }
}
