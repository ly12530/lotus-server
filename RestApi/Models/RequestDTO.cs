using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace RestApi.Models
{
    public class RequestDTO : NewRequestDTO
    {
        [Key]
        public int Id { get; set; }
    }
}