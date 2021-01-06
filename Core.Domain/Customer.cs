using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Core.Domain
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [EmailAddress] 
        public string EmailAddress { get; set; }
        
        public ICollection<Request> Requests { get; set; } = new List<Request>();
    }
}