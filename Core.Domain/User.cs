using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Domain
{
    public class User
    {
        public int Id { get; set; }
        
        public string UserName { get; set; }
        
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        public Role Role { get; set; }
        
        public string Password { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new HashSet<Request>();

        public bool Subscribe(Request request)
        {
            if (request.IsOpen)
            {
                Requests.Add(request);
                return true;
            } else
            {
                return false;
            }
        }
    }
}