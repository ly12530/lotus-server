using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class NewRequestDTO
    {
        public int? CustomerId { get; set; }
        
        [Required]
        [MinLength(2)]
        public string Location { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public bool IsExam { get; set; }

        [Required] 
        public bool IsOpen { get; set; } = false;
        
        [Required]
        public LessonType LessonType { get; set; }
    }
}