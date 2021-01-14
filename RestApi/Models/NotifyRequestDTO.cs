using System.Collections.Generic;

namespace RestApi.Models
{
    public class NotifyRequestDTO
    {
        public int senderId { get; set; }
        
        public IEnumerable<int> receiverIds { get; set; }
    }
}