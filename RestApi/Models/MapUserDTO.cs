using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class MapUserDTO
    {
        [Key]
        public int Id { get; set; }
        
        public string UserName { get; set; }
        
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        public Role Role { get; set; }
        
        public ICollection<MapUserJobsDTO> Jobs { get; set; }

        public class MapUserJobsDTO
        {
            [Key]
            public int Id { get; set; }
            
            public string Title { get; set; }
        }
    }
}