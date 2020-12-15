using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.Domain
{
    public class User
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        [EmailAddress]
        public string EmailAddress { get; set; }
        
        [Phone]
        public string Phone { get; set; }
        
        public Role Role { get; set; }
        
        //TODO ask productowner about all the rights of all roles
        private ICollection<RequestUser>? _requests;
        public ICollection<RequestUser>? Requests
        {
            get => _requests;
            set
            {
                if (this.Role == Role.BettingCoordinator || this.Role == Role.Administrator)
                {
                    _requests ??= new List<RequestUser>();
                    
                    // Don't add values if given value is null
                    if (value == null) return;
                    // Add request to the list of made request
                    _requests.Add(value.First());
                }
            }
        }
    }
}