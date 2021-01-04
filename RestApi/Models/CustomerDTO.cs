using System;
using System.ComponentModel.DataAnnotations;
using Core.Domain;


namespace RestApi.Models
{
    public class CustomerDTO : NewCustomerDTO
    {
        [Key]
        public int Id { get; set; }
    }
}
