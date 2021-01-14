using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class MapUserAuthDTO : JwtToken
    {
        [Key]
        public int Id { get; set; }
        
        public string UserName { get; set; }
        
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        public Role Role { get; set; }
    }
}