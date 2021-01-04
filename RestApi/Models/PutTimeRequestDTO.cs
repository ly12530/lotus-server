using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models
{
    public class PutTimeRequestDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [RegularExpression("^(\\d{2}):(\\d{2})$")]
        public string StartTime { get; set; }

        [Required]
        [RegularExpression("^(\\d{2}):(\\d{2})$")]
        public string EndTime { get; set; }
    }
}
