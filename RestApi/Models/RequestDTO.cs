using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class RequestDTO : NewRequestDTO
    {
        [Key]
        public int Id { get; set; }
        
        public IEnumerable<UserDTO> Subscribers { get; set; }
    }
}