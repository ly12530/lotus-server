using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Core.Domain
{
    public class Request
    {
        public int Id { get; set; }
     
        public string Description { get; set; }
        
        public string Name { get; set; }
        
        public int? DateId { get; set; }
        public RequestDate Date { get; set; }
        
        public bool IsOpen { get; set; }
        
        public ICollection<RequestUser> LotusMembers { get; set; } = new List<RequestUser>();
    }
}