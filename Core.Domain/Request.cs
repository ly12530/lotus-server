using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Domain
{
    public class Request
    {
        public int Id { get; set; }
        
        public Customer Customer { get; set; }
        public int? CustomerId { get; set; }
        
        public string Location { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public bool IsExam { get; set; }
        
        public LessonType LessonType { get; set; }
        
        public bool IsOpen { get; set; }
        
        public ICollection<User>? Instructors { get; set; }
    }
}