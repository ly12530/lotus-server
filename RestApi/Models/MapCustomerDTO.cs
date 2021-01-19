using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class MapCustomerDTO
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        public ICollection<CustomerRequestDTO> Requests { get; set; }

        public class CustomerRequestDTO
        {
            [Key]
            public int Id { get; set; }
            
            public string Title { get; set; }
            
            public CustomerRequestAddressDTO Address { get; set; }
            
            public DateTime Date { get; set; }
            
            public string StartTime { get; set; }

            public string RealStartTime { get; set; }

            public string EndTime { get; set; }

            public string RealEndTime { get; set; }

            public int DistanceTraveled { get; set; }

            public bool IsExam { get; set; }

            public LessonType LessonType { get; set; }
            
            public bool IsOpen { get; set; }
        }

        public class CustomerRequestAddressDTO
        {
            public string Street { get; set; }
            
            public string Number { get; set; }
            
            public string City { get; set; }
            
            public string Postcode { get; set; }
            
            public double[] Geometry { get; set; }
        }
    }
}