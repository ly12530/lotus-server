using System;
using System.Collections.Generic;

namespace Core.Domain
{
    public class Request
    {
        public int Id { get; set; }

        public Customer Customer { get; set; }
        public int? CustomerId { get; set; }

        public Address Address { get; set; }

        //Todo Location weg halen, als het op de front-end kant gelukt is om address toe te passen
        public string Location { get; set; }
        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool IsExam { get; set; }

        public LessonType LessonType { get; set; }

        public bool IsOpen { get; set; }

    //    public ICollection<User>? Instructors { get; set; }

        public virtual ICollection<User> Subscribers { get; set; }  = new HashSet<User>();
        
        // you may also use List<Student>, but HashSet will guarantee that you are not adding the same Student mistakenly twice
        

    }
}