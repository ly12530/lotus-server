using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RestApi.Models
{
    public class NotifyRequestDTO
    {
        [Required]
        public int SenderId { get; set; }
        
        [Required]
        public IEnumerable<int> ReceiverIds { get; set; }
        
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }
        
        [Required]
        [MaxLength(512)]
        public string Body { get; set; }
    }
}