using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class UpdateCustomerPasswordDTO
    {
        [Required]
        public string OldPassword { get; set; }
        
        [Required]
        public string NewPassword { get; set; }
    }
}