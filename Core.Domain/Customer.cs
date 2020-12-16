using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Customer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [EmailAddress] public string EmailAddress { get; set; }
    }
}