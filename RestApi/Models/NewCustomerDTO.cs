using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models
{
    public class NewCustomerDTO
    {

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
