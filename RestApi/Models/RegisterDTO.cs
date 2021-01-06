using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class RegisterDTO
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required]
        public Role Role { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}