using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class UserDTO : RegisterDTO
    {
        [Key] 
        public int Id { get; set; }
    }
}