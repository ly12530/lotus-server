using System.Threading.Tasks;
using Core.Domain;

namespace Core.DomainServices
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerById(int id);
    }
}
