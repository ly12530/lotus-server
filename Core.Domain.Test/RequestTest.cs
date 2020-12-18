using System;
using Xunit;

namespace Core.Domain.Test
{
    public class RequestTest
    {
        [Fact]
        public void Request_Should_Create_The_Right_Way()
        {
            // Arrange
            var customer = new Customer();
            var location = "Breda";
            var startDate = new DateTime();
            var endDate = new DateTime().AddMonths(3);
            var lessonType = LessonType.Normal;
            var isExam = true;

            // Act
            var request = new Request
            {
                Customer = customer,
                Location = location,
                StartDate = startDate,
                EndDate = endDate,
                IsExam = isExam,
                LessonType = lessonType
            };

            // Assert
            Assert.Equal(customer, request.Customer);
            Assert.Equal(location, request.Location);
            Assert.Equal(startDate, request.StartDate);
            Assert.Equal(endDate, request.EndDate);
            Assert.False(request.IsOpen);
            Assert.True(request.IsExam);
            Assert.Equal(lessonType, request.LessonType);
        }

        [Fact]
        public void IsOpen_Should_be_False_At_Creation()
        {
            // Arrange/Act
            var request = new Request();

            // Assert
            Assert.False(request.IsOpen);
        }
    }
}