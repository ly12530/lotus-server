using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly LotusDbContext _context;

        public CustomerRepository(LotusDbContext context)
        {
            _context = context;
        }

        public IQueryable<Customer> GetAllCustomers()
        {
            return _context.Customers;
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            return await _context.Customers.SingleOrDefaultAsync(Customer => Customer.Id == id);
        }
    }
}
