using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}