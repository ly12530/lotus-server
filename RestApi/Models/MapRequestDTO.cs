using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class MapRequestDTO
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public MapRequestCustomer Customer { get; set; }
        
        public Address Address { get; set; }

        public DateTime Date { get; set; }

        public string StartTime { get; set; }

        public string RealStartTime { get; set; }

        public string EndTime { get; set; }

        public string RealEndTime { get; set; }

        public int DistanceTraveled { get; set; }

        public bool IsExam { get; set; }

        public LessonType LessonType { get; set; }

        public bool IsOpen { get; set; }

        //    public ICollection<User>? Instructors { get; set; }

        public virtual ICollection<MapRequestUser> Subscribers { get; set; }  = new HashSet<MapRequestUser>();

        public MapRequestUser DesignatedUser { get; set; }

        public class MapRequestCustomer
        {
            public int Id { get; set; }
            
            public string Name { get; set; }
            
            public string EmailAddress { get; set; }
        }

        public class MapRequestUser
        {
            public int Id { get; set; }
            
            public string UserName { get; set; }
            
            public string EmailAddress { get; set; }
            
            public Role Role { get; set; }
        }
    }
}