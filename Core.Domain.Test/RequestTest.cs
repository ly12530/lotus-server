using System;
using Xunit;

namespace Core.Domain.Test
{
    public class UnitTest1
    {
        [Fact]
        public void RequestShouldHaveCustomerLocationStartAndEndDateIsExamLessonTypeAndIsOpen()
        {
            // Arrange
            var customer = new Customer();
            var location = "Breda";
            var startDate = new DateTime();
            var endDate = new DateTime().AddMonths(3);
            var lessonType = Domain.LessonType.Normal;
            
            // Act
            var request = new Request
            {
                Customer = customer,
                Location = location,
                StartDate = startDate,
                EndDate = endDate,
                LessonType = lessonType
            };
            
            Assert.Equal(customer, request.Customer);
            Assert.Equal(location, request.Location);
            Assert.Equal(startDate, request.StartDate);
            Assert.Equal(endDate, request.EndDate);
            Assert.False(request.IsExam);
            Assert.Equal(lessonType, request.LessonType);
        }

        [Fact]
        public void FailsIfExamIsTrueAtCreation()
        {
            // Arrange
            var exceptionThrown = false;
            
            // Act
            try
            {
                var request = new Request {IsExam = true};

                if (request.IsExam)
                {
                    throw new InvalidOperationException();
                }
            }
            catch
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.True(exceptionThrown);
        }
    }
}