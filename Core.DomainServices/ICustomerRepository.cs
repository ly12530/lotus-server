using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Core.Domain;

namespace Core.DomainServices
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAllCustomers();
        Task RegisterCustomer(Customer newCustomer);
        Task<Customer> GetCustomerById(int id);
        Task<Customer> GetCustomerByEmail(string email);
        Task UpdatePassword(Customer customer);
    }
}
