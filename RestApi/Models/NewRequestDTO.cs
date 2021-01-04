using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class NewRequestDTO
    {
        public int? CustomerId { get; set; }

        [Required] [MinLength(2)] public string Location { get; set; }

        [Required] public NewAddressDTO Address { get; set; }

        [Required] public DateTime Date { get; set; }

        [Required]
        [RegularExpression("^(\\d{2}):(\\d{2})$")]
        public string StartTime { get; set; }

        [Required]
        [RegularExpression("^(\\d{2}):(\\d{2})$")]
        public string EndTime { get; set; }

        [Required] public bool IsExam { get; set; }

        [Required] public bool IsOpen { get; set; } = false;

        [Required] public LessonType LessonType { get; set; }
    }
}