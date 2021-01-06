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

        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public bool IsExam { get; set; }

        public LessonType LessonType { get; set; }

        public bool IsOpen { get; set; }

    //    public ICollection<User>? Instructors { get; set; }

        public virtual ICollection<User> Subscribers { get; set; }  = new HashSet<User>();

        public int Subscribe(User user)
        {
            if (IsOpen)
            {
                if (!Subscribers.Contains(user)) { 
                Subscribers.Add(user);
                return 200; // Succes
                }
                else
                {
                    return 304; //Duplicate
                }
            }
            else
            {
                return 401; // Not authorized
            }
        }
        
        // you may also use List<Student>, but HashSet will guarantee that you are not adding the same Student mistakenly twice
        

    }
}