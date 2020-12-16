using System;

namespace Core.Domain
{
    public class RequestDate
    {
        public int Id { get; set; }
        
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
        
        public int? RequestId { get; set; }
        public Request Request { get; set; }
    }
}