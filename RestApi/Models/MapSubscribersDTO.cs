using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class MapSubscribersDTO
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }
        
        public Role Role { get; set; }
    }
}