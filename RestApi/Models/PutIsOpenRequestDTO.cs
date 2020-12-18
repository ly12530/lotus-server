using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models
{
    public class PutIsOpenRequestDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool IsOpen { get; set; }
    }
}
