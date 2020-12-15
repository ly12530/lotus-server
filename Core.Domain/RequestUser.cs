using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class RequestUser
    {
        public int UserId { get; set; }
        public int RequestId { get; set; }
        
        public User User { get; set; }
        public Request Request { get; set; }
    }
}