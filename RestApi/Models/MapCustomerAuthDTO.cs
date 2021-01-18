using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class MapCustomerAuthDTO : JwtToken
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}