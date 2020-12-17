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
    }
}