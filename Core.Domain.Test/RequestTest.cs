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
            var title = "RequestTitle";
            var customer = new Customer();
            var address = new Address();
            var date = new DateTime();
            var startTime = "11:50";
            var realStartTime = "11:55";
            var endTime = "15:50";
            var realEndTime = "14:30";
            var distanceTraveled = 20;
            var isExam = true;
            var lessonType = LessonType.Normal;

            // Act
            var request = new Request
            {
                Title = title,
                Customer = customer,
                Address = address,
                Date = date,
                StartTime = startTime,
                RealStartTime = realStartTime,
                EndTime = endTime,
                RealEndTime = realEndTime,
                DistanceTraveled = distanceTraveled,
                IsExam = isExam,
                LessonType = lessonType
            };

            // Assert

            Assert.Equal(title, request.Title);
            Assert.Equal(customer, request.Customer);
            Assert.Equal(address, request.Address);
            Assert.Equal(date, request.Date);
            Assert.Equal(startTime, request.StartTime);
            Assert.Equal(realStartTime, request.RealStartTime);
            Assert.Equal(endTime, request.EndTime);
            Assert.Equal(realEndTime, request.RealEndTime);
            Assert.Equal(distanceTraveled, request.DistanceTraveled);
            Assert.True(request.IsExam);
            Assert.Equal(lessonType, request.LessonType);
            Assert.False(request.IsOpen);
        }

        [Fact]
        public void IsOpen_Should_be_False_At_Creation()
        {
            // Arrange/Act
            var request = new Request();

            // Assert
            Assert.False(request.IsOpen);
        }

        [Fact]
        public void User_Should_Only_Be_Allowed_To_Subscribe_When_Request_IsOpen_equals_true()
        {
            // Arrange
            var request = new Request {IsOpen = true};
            var user = new User();

            // Act
            var subscribed = request.Subscribe(user);

            // Assert
            Assert.True(subscribed);
        }

        [Fact]
        public void User_Should_Not_Be_Allowed_To_Subscribe_When_Request_IsOpen_equals_false()
        {
            // Arrange
            var request = new Request {IsOpen = false};
            var user = new User();

            // Act
            var subscribed = request.Subscribe(user);

            // Assert
            Assert.False(subscribed);
        }
    }
}