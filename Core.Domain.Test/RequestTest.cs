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
            var address = new Address();
            var date = new DateTime();
            var startTime = "11:50";
            var endTime = "14:30";
            var lessonType = LessonType.Normal;
            var isExam = true;

            // Act
            var request = new Request
            {
                Customer = customer,
                Location = location,
                Address = address,
                Date = date,
                StartTime = startTime,
                EndTime = endTime,
                IsExam = isExam,
                LessonType = lessonType
            };

            // Assert
            Assert.Equal(customer, request.Customer);
            Assert.Equal(location, request.Location);
            Assert.Equal(address, request.Address);
            Assert.Equal(date, request.Date);
            Assert.Equal(startTime, request.StartTime);
            Assert.Equal(endTime, request.EndTime);
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