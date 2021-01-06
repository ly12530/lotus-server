using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class UserDTO : NewUserDTO
    {
        [Key] 
        public int Id { get; set; }
    }
}